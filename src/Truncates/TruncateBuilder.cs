using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Quark.Enums;
using Soenneker.Utils.PooledStringBuilders;

namespace Soenneker.Quark;

/// <summary>
/// Simplified truncate builder with fluent API for chaining truncate rules.
/// </summary>
public sealed class TruncateBuilder : ICssBuilder
{
    private readonly List<TruncateRule> _rules = new(4);

    private const string _classTextTruncate = "text-truncate";

    internal TruncateBuilder(Breakpoint? breakpoint = null)
    {
        _rules.Add(new TruncateRule(breakpoint));
    }

    internal TruncateBuilder(List<TruncateRule> rules)
    {
        if (rules is { Count: > 0 })
            _rules.AddRange(rules);
    }

    public TruncateBuilder OnPhone => ChainWithBreakpoint(Breakpoint.Phone);
    public TruncateBuilder OnTablet => ChainWithBreakpoint(Breakpoint.Tablet);
    public TruncateBuilder OnLaptop => ChainWithBreakpoint(Breakpoint.Laptop);
    public TruncateBuilder OnDesktop => ChainWithBreakpoint(Breakpoint.Desktop);
    public TruncateBuilder OnWidescreen => ChainWithBreakpoint(Breakpoint.Widescreen);
    public TruncateBuilder OnUltrawide => ChainWithBreakpoint(Breakpoint.Ultrawide);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private TruncateBuilder ChainWithBreakpoint(Breakpoint breakpoint)
    {
        if (_rules.Count == 0)
        {
            _rules.Add(new TruncateRule(breakpoint));
            return this;
        }

        int lastIdx = _rules.Count - 1;
        TruncateRule last = _rules[lastIdx];
        _rules[lastIdx] = new TruncateRule(breakpoint);
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
            TruncateRule rule = _rules[i];
            string cls = _classTextTruncate;

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

            sb.Append("overflow: hidden; text-overflow: ellipsis; white-space: nowrap;");
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
