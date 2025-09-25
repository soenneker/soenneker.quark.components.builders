using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Quark.Enums.Breakpoints;
using Soenneker.Utils.PooledStringBuilders;

namespace Soenneker.Quark;

public sealed class ZIndexBuilder : ICssBuilder
{
    private readonly List<ZIndexRule> _rules = new(4);

    private const string _classNeg1 = "z-n1";
    private const string _class0 = "z-0";
    private const string _class1 = "z-1";
    private const string _class2 = "z-2";
    private const string _class3 = "z-3";

    internal ZIndexBuilder(int value, Breakpoint? breakpoint = null)
    {
        _rules.Add(new ZIndexRule(value, breakpoint));
    }

    internal ZIndexBuilder(List<ZIndexRule> rules)
    {
        if (rules is { Count: > 0 })
            _rules.AddRange(rules);
    }

    public ZIndexBuilder N1 => Chain(-1);
    public ZIndexBuilder Z0 => Chain(0);
    public ZIndexBuilder Z1 => Chain(1);
    public ZIndexBuilder Z2 => Chain(2);
    public ZIndexBuilder Z3 => Chain(3);

    public ZIndexBuilder OnPhone => ChainBp(Breakpoint.Phone);
    public ZIndexBuilder OnTablet => ChainBp(Breakpoint.Tablet);
    public ZIndexBuilder OnLaptop => ChainBp(Breakpoint.Laptop);
    public ZIndexBuilder OnDesktop => ChainBp(Breakpoint.Desktop);
    public ZIndexBuilder OnWidescreen => ChainBp(Breakpoint.Widescreen);
    public ZIndexBuilder OnUltrawide => ChainBp(Breakpoint.Ultrawide);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ZIndexBuilder Chain(int value)
    {
        _rules.Add(new ZIndexRule(value, null));
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ZIndexBuilder ChainBp(Breakpoint bp)
    {
        if (_rules.Count == 0)
        {
            _rules.Add(new ZIndexRule(0, bp));
            return this;
        }

        int lastIdx = _rules.Count - 1;
        ZIndexRule last = _rules[lastIdx];
        _rules[lastIdx] = new ZIndexRule(last.Value, bp);
        return this;
    }

    public string ToClass()
    {
        if (_rules.Count == 0) return string.Empty;

        using var sb = new PooledStringBuilder();
        var first = true;
        for (var i = 0; i < _rules.Count; i++)
        {
            ZIndexRule rule = _rules[i];
            string cls = rule.Value switch
            {
                -1 => _classNeg1,
                0 => _class0,
                1 => _class1,
                2 => _class2,
                3 => _class3,
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
            ZIndexRule rule = _rules[i];
            if (!first) sb.Append("; ");
            else first = false;
            sb.Append("z-index: ");
            sb.Append(rule.Value.ToString());
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

 
