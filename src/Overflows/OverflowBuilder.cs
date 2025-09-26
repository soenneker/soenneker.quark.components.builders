using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Quark.Enums;
using Soenneker.Utils.PooledStringBuilders;

namespace Soenneker.Quark;

/// <summary>
/// High-performance overflow builder with fluent API for chaining overflow rules.
/// </summary>
public sealed class OverflowBuilder : ICssBuilder
{
    private readonly List<OverflowRule> _rules = new(4);

    // ----- Class name constants -----
    private const string _classAuto = "overflow-auto";
    private const string _classHidden = "overflow-hidden";
    private const string _classVisible = "overflow-visible";
    private const string _classScroll = "overflow-scroll";

    // ----- CSS prefix -----
    private const string _overflowPrefix = "overflow: ";

    internal OverflowBuilder(string overflow, Breakpoint? breakpoint = null)
    {
        _rules.Add(new OverflowRule(overflow, breakpoint));
    }

    internal OverflowBuilder(List<OverflowRule> rules)
    {
        if (rules is { Count: > 0 })
            _rules.AddRange(rules);
    }

    // ----- Fluent chaining (values & keywords) -----
    public OverflowBuilder Auto => Chain(Overflow.AutoValue);
    public OverflowBuilder Hidden => Chain(Overflow.HiddenValue);
    public OverflowBuilder Visible => Chain(Overflow.VisibleValue);
    public OverflowBuilder Scroll => Chain(Overflow.ScrollValue);

    public OverflowBuilder Inherit => Chain(GlobalKeyword.InheritValue);
    public OverflowBuilder Initial => Chain(GlobalKeyword.InitialValue);
    public OverflowBuilder Revert => Chain(GlobalKeyword.RevertValue);
    public OverflowBuilder RevertLayer => Chain(GlobalKeyword.RevertLayerValue);
    public OverflowBuilder Unset => Chain(GlobalKeyword.UnsetValue);

    // ----- Breakpoint chaining -----
    public OverflowBuilder OnPhone => ChainBp(Breakpoint.Phone);
    public OverflowBuilder OnTablet => ChainBp(Breakpoint.Tablet);
    public OverflowBuilder OnLaptop => ChainBp(Breakpoint.Laptop);
    public OverflowBuilder OnDesktop => ChainBp(Breakpoint.Desktop);
    public OverflowBuilder OnWidescreen => ChainBp(Breakpoint.Widescreen);
    public OverflowBuilder OnUltrawide => ChainBp(Breakpoint.Ultrawide);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private OverflowBuilder Chain(string overflow)
    {
        _rules.Add(new OverflowRule(overflow, null));
        return this;
    }

    /// <summary>Apply a breakpoint to the most recent rule (or bootstrap with "auto" if empty).</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private OverflowBuilder ChainBp(Breakpoint bp)
    {
        if (_rules.Count == 0)
        {
            _rules.Add(new OverflowRule(Overflow.AutoValue, bp));
            return this;
        }

        int lastIdx = _rules.Count - 1;
        OverflowRule last = _rules[lastIdx];
        _rules[lastIdx] = new OverflowRule(last.Overflow, bp);
        return this;
    }

    /// <summary>Gets the CSS class string for the current configuration.</summary>
    public string ToClass()
    {
        if (_rules.Count == 0)
            return string.Empty;

        using var sb = new PooledStringBuilder();
        var first = true;

        for (var i = 0; i < _rules.Count; i++)
        {
            OverflowRule rule = _rules[i];

            string baseClass = GetOverflowClass(rule.Overflow);
            if (baseClass.Length == 0)
                continue;

            string bp = BreakpointUtil.GetBreakpointToken(rule.Breakpoint);
            if (bp.Length != 0)
                baseClass = InsertBreakpoint(baseClass, bp);

            if (!first) sb.Append(' ');
            else first = false;

            sb.Append(baseClass);
        }

        return sb.ToString();
    }

    /// <summary>Gets the CSS style string for the current configuration.</summary>
    public string ToStyle()
    {
        if (_rules.Count == 0)
            return string.Empty;

        using var sb = new PooledStringBuilder();
        var first = true;

        for (var i = 0; i < _rules.Count; i++)
        {
            string value = _rules[i].Overflow;
            if (string.IsNullOrEmpty(value))
                continue;

            if (!first) sb.Append("; ");
            else first = false;

            sb.Append(_overflowPrefix);
            sb.Append(value);
        }

        return sb.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetOverflowClass(string overflow)
    {
        return overflow switch
        {
            Overflow.AutoValue => _classAuto,
            Overflow.HiddenValue => _classHidden,
            Overflow.VisibleValue => _classVisible,
            Overflow.ScrollValue => _classScroll,
            _ => string.Empty
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetBp(Breakpoint? breakpoint) => breakpoint?.Value ?? string.Empty;

    /// <summary>
    /// Insert breakpoint token as: "overflow-hidden" + "md" ? "overflow-md-hidden".
    /// Falls back to "bp-{class}" if no dash exists.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string InsertBreakpoint(string className, string bp)
    {
        int dashIndex = className.IndexOf('-');
        if (dashIndex > 0)
        {
            // length = prefix + "-" + bp + remainder
            int len = dashIndex + 1 + bp.Length + (className.Length - dashIndex);
            return string.Create(len, (className, dashIndex, bp), static (dst, s) =>
            {
                // prefix
                s.className.AsSpan(0, s.dashIndex).CopyTo(dst);
                int idx = s.dashIndex;

                // "-" + bp
                dst[idx++] = '-';
                s.bp.AsSpan().CopyTo(dst[idx..]);
                idx += s.bp.Length;

                // remainder (starts with '-')
                s.className.AsSpan(s.dashIndex).CopyTo(dst[idx..]);
            });
        }

        // Fallback: "bp-{className}"
        return string.Create(bp.Length + 1 + className.Length, (className, bp), static (dst, s) =>
        {
            s.bp.AsSpan().CopyTo(dst);
            int idx = s.bp.Length;
            dst[idx++] = '-';
            s.className.AsSpan().CopyTo(dst[idx..]);
        });
    }
}
