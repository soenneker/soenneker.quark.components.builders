using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Quark.Enums;
using Soenneker.Utils.PooledStringBuilders;

namespace Soenneker.Quark;

/// <summary>
/// Simplified stretched link builder with fluent API for chaining stretched link rules.
/// </summary>
public sealed class StretchedLinkBuilder : ICssBuilder
{
    private readonly List<StretchedLinkRule> _rules = new(4);

    private const string _classStretchedLink = "stretched-link";

    internal StretchedLinkBuilder(Breakpoint? breakpoint = null)
    {
        _rules.Add(new StretchedLinkRule(breakpoint));
    }

    internal StretchedLinkBuilder(List<StretchedLinkRule> rules)
    {
        if (rules is { Count: > 0 })
            _rules.AddRange(rules);
    }

    public StretchedLinkBuilder OnPhone => ChainWithBreakpoint(Breakpoint.Phone);
    public StretchedLinkBuilder OnTablet => ChainWithBreakpoint(Breakpoint.Tablet);
    public StretchedLinkBuilder OnLaptop => ChainWithBreakpoint(Breakpoint.Laptop);
    public StretchedLinkBuilder OnDesktop => ChainWithBreakpoint(Breakpoint.Desktop);
    public StretchedLinkBuilder OnWidescreen => ChainWithBreakpoint(Breakpoint.Widescreen);
    public StretchedLinkBuilder OnUltrawide => ChainWithBreakpoint(Breakpoint.Ultrawide);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private StretchedLinkBuilder ChainWithBreakpoint(Breakpoint breakpoint)
    {
        if (_rules.Count == 0)
        {
            _rules.Add(new StretchedLinkRule(breakpoint));
            return this;
        }

        int lastIdx = _rules.Count - 1;
        StretchedLinkRule last = _rules[lastIdx];
        _rules[lastIdx] = new StretchedLinkRule(breakpoint);
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
            StretchedLinkRule rule = _rules[i];
            string cls = _classStretchedLink;

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

            sb.Append("position: absolute; top: 0; right: 0; bottom: 0; left: 0; z-index: 1; pointer-events: auto; content: \"\";");
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
