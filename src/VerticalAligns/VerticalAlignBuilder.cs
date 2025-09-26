using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Quark.Enums;
using Soenneker.Utils.PooledStringBuilders;

namespace Soenneker.Quark;

public sealed class VerticalAlignBuilder : ICssBuilder
{
    private readonly List<VerticalAlignRule> _rules = new(6);

    internal VerticalAlignBuilder(string value, Breakpoint? breakpoint = null)
    {
        _rules.Add(new VerticalAlignRule(value, breakpoint));
    }

    internal VerticalAlignBuilder(List<VerticalAlignRule> rules)
    {
        if (rules is { Count: > 0 })
            _rules.AddRange(rules);
    }

    public VerticalAlignBuilder Baseline => Chain(Enums.VerticalAlign.BaselineValue);
    public VerticalAlignBuilder Top => Chain(Enums.VerticalAlign.TopValue);
    public VerticalAlignBuilder Middle => Chain(Enums.VerticalAlign.MiddleValue);
    public VerticalAlignBuilder Bottom => Chain(Enums.VerticalAlign.BottomValue);
    public VerticalAlignBuilder TextTop => Chain(Enums.VerticalAlign.TextTopValue);
    public VerticalAlignBuilder TextBottom => Chain(Enums.VerticalAlign.TextBottomValue);
    public VerticalAlignBuilder Inherit => Chain(GlobalKeyword.InheritValue);
    public VerticalAlignBuilder Initial => Chain(GlobalKeyword.InitialValue);
    public VerticalAlignBuilder Revert => Chain(GlobalKeyword.RevertValue);
    public VerticalAlignBuilder RevertLayer => Chain(GlobalKeyword.RevertLayerValue);
    public VerticalAlignBuilder Unset => Chain(GlobalKeyword.UnsetValue);

    public VerticalAlignBuilder OnPhone => ChainBp(Breakpoint.Phone);
    public VerticalAlignBuilder OnTablet => ChainBp(Breakpoint.Tablet);
    public VerticalAlignBuilder OnLaptop => ChainBp(Breakpoint.Laptop);
    public VerticalAlignBuilder OnDesktop => ChainBp(Breakpoint.Desktop);
    public VerticalAlignBuilder OnWidescreen => ChainBp(Breakpoint.Widescreen);
    public VerticalAlignBuilder OnUltrawide => ChainBp(Breakpoint.Ultrawide);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private VerticalAlignBuilder Chain(string value)
    {
        _rules.Add(new VerticalAlignRule(value, null));
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private VerticalAlignBuilder ChainBp(Breakpoint bp)
    {
        if (_rules.Count == 0)
        {
            _rules.Add(new VerticalAlignRule(Enums.VerticalAlign.BaselineValue, bp));
            return this;
        }

        int lastIdx = _rules.Count - 1;
        VerticalAlignRule last = _rules[lastIdx];
        _rules[lastIdx] = new VerticalAlignRule(last.Value, bp);
        return this;
    }

    public string ToClass()
    {
        if (_rules.Count == 0) return string.Empty;

        using var sb = new PooledStringBuilder();
        var first = true;
        for (var i = 0; i < _rules.Count; i++)
        {
            VerticalAlignRule rule = _rules[i];
            string cls = rule.Value switch
            {
                Enums.VerticalAlign.BaselineValue => "align-baseline",
                Enums.VerticalAlign.TopValue => "align-top",
                Enums.VerticalAlign.MiddleValue => "align-middle",
                Enums.VerticalAlign.BottomValue => "align-bottom",
                Enums.VerticalAlign.TextTopValue => "align-text-top",
                Enums.VerticalAlign.TextBottomValue => "align-text-bottom",
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
            VerticalAlignRule rule = _rules[i];
            string val = rule.Value;
            if (string.IsNullOrEmpty(val))
                continue;

            if (!first) sb.Append("; ");
            else first = false;

            sb.Append("vertical-align: ");
            sb.Append(val);
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

 
