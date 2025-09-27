using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Quark.Enums;
using Soenneker.Utils.PooledStringBuilders;

namespace Soenneker.Quark;

public sealed class LineHeightBuilder : ICssBuilder
{
    private readonly List<LineHeightRule> _rules = new(4);

    private const string _classLh1 = "lh-1";
    private const string _classLhSm = "lh-sm";
    private const string _classLhBase = "lh-base";
    private const string _classLhLg = "lh-lg";

    internal LineHeightBuilder(string value, Breakpoint? breakpoint = null)
    {
        _rules.Add(new LineHeightRule(value, breakpoint));
    }

    internal LineHeightBuilder(List<LineHeightRule> rules)
    {
        if (rules is { Count: > 0 })
            _rules.AddRange(rules);
    }

    public LineHeightBuilder L1 => Chain(ScaleType.S1.Value);
    public LineHeightBuilder Sm => Chain(SizeType.Small.Value);
    public LineHeightBuilder Base => Chain("base");
    public LineHeightBuilder Lg => Chain(SizeType.Large.Value);

    public LineHeightBuilder OnPhone => ChainBp(Breakpoint.Phone);
    public LineHeightBuilder OnTablet => ChainBp(Breakpoint.Tablet);
    public LineHeightBuilder OnLaptop => ChainBp(Breakpoint.Laptop);
    public LineHeightBuilder OnDesktop => ChainBp(Breakpoint.Desktop);
    public LineHeightBuilder OnWidescreen => ChainBp(Breakpoint.Widescreen);
    public LineHeightBuilder OnUltrawide => ChainBp(Breakpoint.Ultrawide);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private LineHeightBuilder Chain(string value)
    {
        _rules.Add(new LineHeightRule(value, null));
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private LineHeightBuilder ChainBp(Breakpoint bp)
    {
        if (_rules.Count == 0)
        {
            _rules.Add(new LineHeightRule("base", bp));
            return this;
        }

        int lastIdx = _rules.Count - 1;
        LineHeightRule last = _rules[lastIdx];
        _rules[lastIdx] = new LineHeightRule(last.Value, bp);
        return this;
    }

    public string ToClass()
    {
        if (_rules.Count == 0) return string.Empty;

        using var sb = new PooledStringBuilder();
        var first = true;
        for (var i = 0; i < _rules.Count; i++)
        {
            LineHeightRule rule = _rules[i];
            string cls = rule.Value switch
            {
                ScaleType.S1Value => _classLh1,
                "sm" => _classLhSm,
                "base" => _classLhBase,
                "lg" => _classLhLg,
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
            LineHeightRule rule = _rules[i];
            string? css = rule.Value switch
            {
                ScaleType.S1Value => "1",
                "sm" => "1.25",
                "base" => "1.5",
                "lg" => "2",
                _ => null
            };
            if (css is null) continue;

            if (!first) sb.Append("; ");
            else first = false;

            sb.Append("line-height: ");
            sb.Append(css);
        }
        return sb.ToString();
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

 
