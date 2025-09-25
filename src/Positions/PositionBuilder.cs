using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Quark.Enums.Breakpoints;
using Soenneker.Utils.PooledStringBuilders;

namespace Soenneker.Quark;

/// <summary>
/// High-performance position builder with fluent API for chaining position rules.
/// </summary>
public sealed class PositionBuilder : ICssBuilder
{
    private readonly List<PositionRule> _rules = new(4);

    // ----- Class name constants -----
    private const string _classStatic = "position-static";
    private const string _classRelative = "position-relative";
    private const string _classAbsolute = "position-absolute";
    private const string _classFixed = "position-fixed";
    private const string _classSticky = "position-sticky";

    // ----- CSS prefix -----
    private const string _positionPrefix = "position: ";

    internal PositionBuilder(string position, Breakpoint? breakpoint = null)
    {
        _rules.Add(new PositionRule(position, breakpoint));
    }

    internal PositionBuilder(List<PositionRule> rules)
    {
        if (rules is { Count: > 0 })
            _rules.AddRange(rules);
    }

    /// <summary>Chain with static positioning for the next rule.</summary>
    public PositionBuilder Static => ChainWithPosition(Enums.Positions.Position.StaticValue);
    /// <summary>Chain with relative positioning for the next rule.</summary>
    public PositionBuilder Relative => ChainWithPosition(Enums.Positions.Position.RelativeValue);
    /// <summary>Chain with absolute positioning for the next rule.</summary>
    public PositionBuilder Absolute => ChainWithPosition(Enums.Positions.Position.AbsoluteValue);
    /// <summary>Chain with fixed positioning for the next rule.</summary>
    public PositionBuilder Fixed => ChainWithPosition(Enums.Positions.Position.FixedValue);
    /// <summary>Chain with sticky positioning for the next rule.</summary>
    public PositionBuilder Sticky => ChainWithPosition(Enums.Positions.Position.StickyValue);

    public PositionBuilder Inherit => ChainWithPosition(Enums.GlobalKeywords.GlobalKeyword.InheritValue);
    public PositionBuilder Initial => ChainWithPosition(Enums.GlobalKeywords.GlobalKeyword.InitialValue);
    public PositionBuilder Revert => ChainWithPosition(Enums.GlobalKeywords.GlobalKeyword.RevertValue);
    public PositionBuilder RevertLayer => ChainWithPosition(Enums.GlobalKeywords.GlobalKeyword.RevertLayerValue);
    public PositionBuilder Unset => ChainWithPosition(Enums.GlobalKeywords.GlobalKeyword.UnsetValue);

    // ----- Breakpoint chaining -----
    public PositionBuilder OnPhone => ChainWithBreakpoint(Breakpoint.Phone);
    public PositionBuilder OnTablet => ChainWithBreakpoint(Breakpoint.Tablet);
    public PositionBuilder OnLaptop => ChainWithBreakpoint(Breakpoint.Laptop);
    public PositionBuilder OnDesktop => ChainWithBreakpoint(Breakpoint.Desktop);
    public PositionBuilder OnWidescreen => ChainWithBreakpoint(Breakpoint.Widescreen);
    public PositionBuilder OnUltrawide => ChainWithBreakpoint(Breakpoint.Ultrawide);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private PositionBuilder ChainWithPosition(string position)
    {
        _rules.Add(new PositionRule(position, null));
        return this;
    }

    /// <summary>Apply a breakpoint to the most recent rule (or bootstrap with "static" if empty).</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private PositionBuilder ChainWithBreakpoint(Breakpoint breakpoint)
    {
        if (_rules.Count == 0)
        {
            _rules.Add(new PositionRule(Enums.Positions.Position.StaticValue, breakpoint));
            return this;
        }

        int lastIdx = _rules.Count - 1;
        PositionRule last = _rules[lastIdx];
        _rules[lastIdx] = new PositionRule(last.Position, breakpoint);
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
            PositionRule rule = _rules[i];

            string baseClass = GetPositionClass(rule.Position);
            if (baseClass.Length == 0)
                continue;

            string bp = BreakpointUtil.GetBreakpointToken(rule.Breakpoint);
            if (bp.Length != 0)
                baseClass = InsertBreakpoint(baseClass, bp);

            if (!first)
                sb.Append(' ');
            else
                first = false;

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
            PositionRule rule = _rules[i];
            if (rule.Position.Length == 0)
                continue;

            if (!first)
                sb.Append("; ");
            else
                first = false;

            sb.Append(_positionPrefix);
            sb.Append(rule.Position);
        }

        return sb.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetPositionClass(string position)
    {
        return position switch
        {
            // Intellenum<string> *Value constants are compile-time consts, safe in switch
            Enums.Positions.Position.StaticValue => _classStatic,
            Enums.Positions.Position.RelativeValue => _classRelative,
            Enums.Positions.Position.AbsoluteValue => _classAbsolute,
            Enums.Positions.Position.FixedValue => _classFixed,
            Enums.Positions.Position.StickyValue => _classSticky,
            _ => string.Empty
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetBp(Breakpoint? breakpoint) => breakpoint?.Value ?? string.Empty;

    /// <summary>
    /// Insert breakpoint token as: "position-fixed" + "md" ? "position-md-fixed".
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
