using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Quark.Components.Builders.Abstract;
using Soenneker.Quark.Components.Builders.Utils;
using Soenneker.Quark.Enums.Breakpoints;
using Soenneker.Utils.PooledStringBuilders;

namespace Soenneker.Quark.Components.Builders.Colors;

/// <summary>
/// High-performance builder for text/background colors.
/// Produces Bootstrap utility classes when possible, otherwise falls back to inline style.
/// </summary>
public sealed class ColorBuilder : ICssBuilder
{
    private readonly List<ColorRule> _rules = new(4);

    // Class name constants
    private const string _textPrefix = "text";
    private const string _bgPrefix = "bg";

    // CSS property names
    private const string _cssText = "color: ";
    private const string _cssBg = "background-color: ";

    internal ColorBuilder(string value, bool background = false, Breakpoint? breakpoint = null)
    {
        _rules.Add(new ColorRule(value, background, breakpoint));
    }

    internal ColorBuilder(List<ColorRule> rules)
    {
        if (rules is { Count: > 0 })
            _rules.AddRange(rules);
    }

    // Fluent chaining
    public ColorBuilder Text => ChainBackground(false);
    public ColorBuilder Background => ChainBackground(true);

    public ColorBuilder Primary => ChainValue("primary");
    public ColorBuilder Secondary => ChainValue("secondary");
    public ColorBuilder Success => ChainValue("success");
    public ColorBuilder Danger => ChainValue("danger");
    public ColorBuilder Warning => ChainValue("warning");
    public ColorBuilder Info => ChainValue("info");
    public ColorBuilder Light => ChainValue("light");
    public ColorBuilder Dark => ChainValue("dark");
    public ColorBuilder Link => ChainValue("link");
    public ColorBuilder Muted => ChainValue("muted");

    public ColorBuilder Inherit => ChainValue(Enums.GlobalKeywords.GlobalKeyword.InheritValue);
    public ColorBuilder Initial => ChainValue(Enums.GlobalKeywords.GlobalKeyword.InitialValue);
    public ColorBuilder Revert => ChainValue(Enums.GlobalKeywords.GlobalKeyword.RevertValue);
    public ColorBuilder RevertLayer => ChainValue(Enums.GlobalKeywords.GlobalKeyword.RevertLayerValue);
    public ColorBuilder Unset => ChainValue(Enums.GlobalKeywords.GlobalKeyword.UnsetValue);

    public ColorBuilder OnPhone => ChainBp(Breakpoint.Phone);
    public ColorBuilder OnTablet => ChainBp(Breakpoint.Tablet);
    public ColorBuilder OnLaptop => ChainBp(Breakpoint.Laptop);
    public ColorBuilder OnDesktop => ChainBp(Breakpoint.Desktop);
    public ColorBuilder OnWidescreen => ChainBp(Breakpoint.Widescreen);
    public ColorBuilder OnUltrawide => ChainBp(Breakpoint.Ultrawide);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ColorBuilder ChainBackground(bool background)
    {
        if (_rules.Count == 0)
        {
            _rules.Add(new ColorRule("inherit", background, null));
            return this;
        }

        int last = _rules.Count - 1;
        ColorRule prev = _rules[last];
        _rules[last] = new ColorRule(prev.Value, background, prev.Breakpoint);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ColorBuilder ChainValue(string value)
    {
        _rules.Add(new ColorRule(value, _rules.Count > 0 && _rules[^1].Background, null));
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ColorBuilder ChainBp(Breakpoint bp)
    {
        if (_rules.Count == 0)
        {
            _rules.Add(new ColorRule("inherit", false, bp));
            return this;
        }

        int lastIdx = _rules.Count - 1;
        ColorRule last = _rules[lastIdx];
        _rules[lastIdx] = new ColorRule(last.Value, last.Background, bp);
        return this;
    }

    public string ToClass()
    {
        if (_rules.Count == 0)
            return string.Empty;

        using var sb = new PooledStringBuilder();
        var first = true;

        for (var i = 0; i < _rules.Count; i++)
        {
            ColorRule rule = _rules[i];
            string cls = GetClass(rule);
            if (cls.Length == 0)
                continue;

            string bp = BreakpointUtil.GetBreakpointToken(rule.Breakpoint);
            if (bp.Length != 0)
                cls = InsertBreakpoint(cls, bp);

            if (!first) sb.Append(' ');
            else first = false;

            sb.Append(cls);
        }

        return sb.ToString();
    }

    public string ToStyle()
    {
        if (_rules.Count == 0)
            return string.Empty;

        using var sb = new PooledStringBuilder();
        var first = true;

        for (var i = 0; i < _rules.Count; i++)
        {
            ColorRule rule = _rules[i];
            string? css = GetStyle(rule);
            if (css is null)
                continue;

            if (!first) sb.Append("; ");
            else first = false;

            sb.Append(css);
        }

        return sb.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetClass(ColorRule rule)
    {
        // Only known theme tokens map to utility classes; CSS keywords go to style.
        switch (rule.Value)
        {
            case "primary":
            case "secondary":
            case "success":
            case "danger":
            case "warning":
            case "info":
            case "light":
            case "dark":
            case "link":
            case "muted":
                return rule.Background ? $"{_bgPrefix}-{rule.Value}" : $"{_textPrefix}-{rule.Value}";

            default:
                return string.Empty;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string? GetStyle(ColorRule rule)
    {
        // If not a known theme token, treat as raw CSS value (including CSS-wide keywords)
        switch (rule.Value)
        {
            case "primary":
            case "secondary":
            case "success":
            case "danger":
            case "warning":
            case "info":
            case "light":
            case "dark":
            case "link":
            case "muted":
                return null;
            default:
                return rule.Background ? _cssBg + rule.Value : _cssText + rule.Value;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string InsertBreakpoint(string className, string bp)
    {
        int dashIndex = className.IndexOf('-');
        if (dashIndex > 0)
        {
            int len = dashIndex + 1 + bp.Length + (className.Length - dashIndex);
            return string.Create(len, (className, dashIndex, bp), static (dst, s) =>
            {
                s.className.AsSpan(0, s.dashIndex).CopyTo(dst);
                int idx = s.dashIndex;
                dst[idx++] = '-';
                s.bp.AsSpan().CopyTo(dst[idx..]);
                idx += s.bp.Length;
                s.className.AsSpan(s.dashIndex).CopyTo(dst[idx..]);
            });
        }

        return string.Create(bp.Length + 1 + className.Length, (className, bp), static (dst, s) =>
        {
            s.bp.AsSpan().CopyTo(dst);
            int idx = s.bp.Length;
            dst[idx++] = '-';
            s.className.AsSpan().CopyTo(dst[idx..]);
        });
    }
}