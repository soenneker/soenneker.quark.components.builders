using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Extensions.String;
using Soenneker.Utils.PooledStringBuilders;

namespace Soenneker.Quark;

/// <summary>
/// High-performance generic color builder.
/// Produces Bootstrap utility classes when possible, otherwise falls back to inline style.
/// </summary>
public sealed class ColorBuilder : ICssBuilder
{
    private readonly List<ColorRule> _rules = new(4);

    internal ColorBuilder(string value, Breakpoint? breakpoint = null)
    {
        _rules.Add(new ColorRule(value, breakpoint));
    }

    internal ColorBuilder(List<ColorRule> rules)
    {
        if (rules is { Count: > 0 })
            _rules.AddRange(rules);
    }
    
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
    private ColorBuilder ChainValue(string value)
    {
        _rules.Add(new ColorRule(value, null));
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ColorBuilder ChainBp(Breakpoint bp)
    {
        if (_rules.Count == 0)
        {
            _rules.Add(new ColorRule("inherit", bp));
            return this;
        }

        int lastIdx = _rules.Count - 1;
        ColorRule last = _rules[lastIdx];
        _rules[lastIdx] = new ColorRule(last.Value, bp);
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

    /// <summary>
    /// Returns the string representation of the color builder.
    /// Chooses between class and style based on whether any rules generate CSS classes.
    /// </summary>
    public override string ToString()
    {
        if (_rules.Count == 0)
            return string.Empty;

        // First try to generate classes
        string classResult = ToClass();
        if (classResult.HasContent())
            return classResult;

        // Fall back to styles if no classes were generated
        return ToStyle();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetClass(ColorRule rule)
    {
        // Only known theme tokens map to utility classes
        return rule.Value switch
        {
            "primary" or "secondary" or "success" or "danger" or "warning" or "info" or "light" or "dark" or "link" or "muted" => rule.Value,
            _ => string.Empty
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string? GetStyle(ColorRule rule)
    {
        // If not a known theme token, treat as raw CSS value
        return rule.Value switch
        {
            "primary" or "secondary" or "success" or "danger" or "warning" or "info" or "light" or "dark" or "link" or "muted" => null,
            _ => rule.Value
        };
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
