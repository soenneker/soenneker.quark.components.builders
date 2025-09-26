using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Quark.Enums;
using Soenneker.Utils.PooledStringBuilders;

namespace Soenneker.Quark;

/// <summary>
/// Simplified clearfix builder with fluent API for chaining clearfix rules.
/// </summary>
public sealed class ClearfixBuilder : ICssBuilder
{
    private readonly List<ClearfixRule> _rules = new(4);

    private const string _classClearfix = "clearfix";

    internal ClearfixBuilder(Breakpoint? breakpoint = null)
    {
        _rules.Add(new ClearfixRule(breakpoint));
    }

    internal ClearfixBuilder(List<ClearfixRule> rules)
    {
        if (rules is { Count: > 0 })
            _rules.AddRange(rules);
    }

    public ClearfixBuilder OnPhone => ChainWithBreakpoint(Breakpoint.Phone);
    public ClearfixBuilder OnTablet => ChainWithBreakpoint(Breakpoint.Tablet);
    public ClearfixBuilder OnLaptop => ChainWithBreakpoint(Breakpoint.Laptop);
    public ClearfixBuilder OnDesktop => ChainWithBreakpoint(Breakpoint.Desktop);
    public ClearfixBuilder OnWidescreen => ChainWithBreakpoint(Breakpoint.Widescreen);
    public ClearfixBuilder OnUltrawide => ChainWithBreakpoint(Breakpoint.Ultrawide);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ClearfixBuilder ChainWithBreakpoint(Breakpoint breakpoint)
    {
        if (_rules.Count == 0)
        {
            _rules.Add(new ClearfixRule(breakpoint));
            return this;
        }

        int lastIdx = _rules.Count - 1;
        ClearfixRule last = _rules[lastIdx];
        _rules[lastIdx] = new ClearfixRule(breakpoint);
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
            ClearfixRule rule = _rules[i];
            string cls = _classClearfix;

            string bp = BreakpointUtil.GetBreakpointClass(rule.Breakpoint);
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
            if (!first) sb.Append("; ");
            else first = false;

            sb.Append("&::after { display: block; clear: both; content: \"\"; }");
        }

        return sb.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string InsertBreakpoint(string className, string bp)
    {
        return string.Create(bp.Length + 1 + className.Length, (className, bp), static (dst, s) =>
        {
            s.bp.AsSpan().CopyTo(dst);
            int idx = s.bp.Length;
            dst[idx++] = '-';
            s.className.AsSpan().CopyTo(dst[idx..]);
        });
    }

    public override string ToString()
    {
        return ToClass();
    }
}
