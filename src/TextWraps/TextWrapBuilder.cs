using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Utils.PooledStringBuilders;

namespace Soenneker.Quark;

public sealed class TextWrapBuilder : ICssBuilder
{
    private readonly List<TextWrapRule> _rules = new(4);

    private const string _classWrap = "text-wrap";
    private const string _classNoWrap = "text-nowrap";
    private const string _stylePrefix = "text-wrap: ";

    internal TextWrapBuilder(string value, Breakpoint? breakpoint = null)
    {
        _rules.Add(new TextWrapRule(value, breakpoint));
    }

    internal TextWrapBuilder(List<TextWrapRule> rules)
    {
        if (rules is { Count: > 0 })
            _rules.AddRange(rules);
    }

    public TextWrapBuilder Wrap => Chain(TextWrap.WrapValue);
    public TextWrapBuilder NoWrap => Chain(TextWrap.NoWrapValue);
    public TextWrapBuilder Inherit => Chain(GlobalKeyword.InheritValue);
    public TextWrapBuilder Initial => Chain(GlobalKeyword.InitialValue);
    public TextWrapBuilder Revert => Chain(GlobalKeyword.RevertValue);
    public TextWrapBuilder RevertLayer => Chain(GlobalKeyword.RevertLayerValue);
    public TextWrapBuilder Unset => Chain(GlobalKeyword.UnsetValue);

    public TextWrapBuilder OnPhone => ChainBp(Breakpoint.Phone);
    public TextWrapBuilder OnTablet => ChainBp(Breakpoint.Tablet);
    public TextWrapBuilder OnLaptop => ChainBp(Breakpoint.Laptop);
    public TextWrapBuilder OnDesktop => ChainBp(Breakpoint.Desktop);
    public TextWrapBuilder OnWidescreen => ChainBp(Breakpoint.Widescreen);
    public TextWrapBuilder OnUltrawide => ChainBp(Breakpoint.Ultrawide);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private TextWrapBuilder Chain(string value)
    {
        _rules.Add(new TextWrapRule(value, null));
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private TextWrapBuilder ChainBp(Breakpoint bp)
    {
        if (_rules.Count == 0)
        {
            _rules.Add(new TextWrapRule(TextWrap.WrapValue, bp));
            return this;
        }

        int lastIdx = _rules.Count - 1;
        TextWrapRule last = _rules[lastIdx];
        _rules[lastIdx] = new TextWrapRule(last.Value, bp);
        return this;
    }

    public string ToClass()
    {
        if (_rules.Count == 0) return string.Empty;

        using var sb = new PooledStringBuilder();
        var first = true;

        for (var i = 0; i < _rules.Count; i++)
        {
            TextWrapRule rule = _rules[i];
            string cls = rule.Value switch
            {
                TextWrap.WrapValue => _classWrap,
                TextWrap.NoWrapValue => _classNoWrap,
                _ => string.Empty
            };

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
        if (_rules.Count == 0) return string.Empty;

        using var sb = new PooledStringBuilder();
        var first = true;

        for (var i = 0; i < _rules.Count; i++)
        {
            TextWrapRule rule = _rules[i];
            string? css = rule.Value switch
            {
                TextWrap.WrapValue => "wrap",
                TextWrap.NoWrapValue => "nowrap",
                GlobalKeyword.InheritValue => "inherit",
                GlobalKeyword.InitialValue => "initial",
                GlobalKeyword.UnsetValue => "unset",
                GlobalKeyword.RevertValue => "revert",
                GlobalKeyword.RevertLayerValue => "revert-layer",
                _ => null
            };

            if (css is null)
                continue;

            if (!first) sb.Append("; ");
            else first = false;

            sb.Append(_stylePrefix);
            sb.Append(css);
        }

        return sb.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetBp(Breakpoint? breakpoint) => breakpoint?.Value ?? string.Empty;

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

 
