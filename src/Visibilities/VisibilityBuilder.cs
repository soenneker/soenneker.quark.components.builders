using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Quark.Enums;
using Soenneker.Utils.PooledStringBuilders;

namespace Soenneker.Quark;

public sealed class VisibilityBuilder : ICssBuilder
{
    private readonly List<VisibilityRule> _rules = new(4);

    private const string _classInvisible = "invisible";
    private const string _classVisible = "visible";

    internal VisibilityBuilder(string value, Breakpoint? breakpoint = null)
    {
        _rules.Add(new VisibilityRule(value, breakpoint));
    }

    internal VisibilityBuilder(List<VisibilityRule> rules)
    {
        if (rules is { Count: > 0 })
            _rules.AddRange(rules);
    }

    public VisibilityBuilder Visible => Chain(VisibilityKeyword.VisibleValue);
    public VisibilityBuilder Invisible => Chain("invisible");
    public VisibilityBuilder Inherit => Chain(GlobalKeyword.InheritValue);
    public VisibilityBuilder Initial => Chain(GlobalKeyword.InitialValue);
    public VisibilityBuilder Revert => Chain(GlobalKeyword.RevertValue);
    public VisibilityBuilder RevertLayer => Chain(GlobalKeyword.RevertLayerValue);
    public VisibilityBuilder Unset => Chain(GlobalKeyword.UnsetValue);

    public VisibilityBuilder OnPhone => ChainBp(Breakpoint.Phone);
    public VisibilityBuilder OnTablet => ChainBp(Breakpoint.Tablet);
    public VisibilityBuilder OnLaptop => ChainBp(Breakpoint.Laptop);
    public VisibilityBuilder OnDesktop => ChainBp(Breakpoint.Desktop);
    public VisibilityBuilder OnWidescreen => ChainBp(Breakpoint.Widescreen);
    public VisibilityBuilder OnUltrawide => ChainBp(Breakpoint.Ultrawide);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private VisibilityBuilder Chain(string value)
    {
        _rules.Add(new VisibilityRule(value, null));
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private VisibilityBuilder ChainBp(Breakpoint bp)
    {
        if (_rules.Count == 0)
        {
            _rules.Add(new VisibilityRule(VisibilityKeyword.VisibleValue, bp));
            return this;
        }

        int lastIdx = _rules.Count - 1;
        VisibilityRule last = _rules[lastIdx];
        _rules[lastIdx] = new VisibilityRule(last.Value, bp);
        return this;
    }

    public string ToClass()
    {
        if (_rules.Count == 0) return string.Empty;

        using var sb = new PooledStringBuilder();
        var first = true;
        for (var i = 0; i < _rules.Count; i++)
        {
            VisibilityRule rule = _rules[i];
            string cls = rule.Value switch
            {
                "invisible" => _classInvisible,
                VisibilityKeyword.VisibleValue => _classVisible,
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
            VisibilityRule rule = _rules[i];
            string? css = rule.Value switch
            {
                "invisible" => "visibility: hidden",
                VisibilityKeyword.VisibleValue => "visibility: visible",
                GlobalKeyword.InheritValue => "visibility: inherit",
                GlobalKeyword.InitialValue => "visibility: initial",
                GlobalKeyword.UnsetValue => "visibility: unset",
                GlobalKeyword.RevertValue => "visibility: revert",
                GlobalKeyword.RevertLayerValue => "visibility: revert-layer",
                _ => null
            };
            if (css is null) continue;

            if (!first) sb.Append("; ");
            else first = false;

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

 
