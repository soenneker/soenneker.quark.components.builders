using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Utils.PooledStringBuilders;

namespace Soenneker.Quark;

/// <summary>
/// Simplified float builder with fluent API for chaining float rules.
/// </summary>
public sealed class FloatBuilder : ICssBuilder
{
    private readonly List<FloatRule> _rules = new(4);

    private const string _classStart = "float-start";
    private const string _classEnd = "float-end";
    private const string _classNone = "float-none";

    private const string _floatPrefix = "float: ";

    internal FloatBuilder(string value, Breakpoint? breakpoint = null)
    {
        _rules.Add(new FloatRule(value, breakpoint));
    }

    internal FloatBuilder(List<FloatRule> rules)
    {
        if (rules is { Count: > 0 })
            _rules.AddRange(rules);
    }

    public FloatBuilder None => ChainWithValue(Float.NoneValue);
    public FloatBuilder Left => ChainWithValue(Float.LeftValue);
    public FloatBuilder Right => ChainWithValue(Float.RightValue);
    public FloatBuilder Start => ChainWithValue(Float.InlineStartValue);
    public FloatBuilder End => ChainWithValue(Float.InlineEndValue);
    public FloatBuilder Inherit => ChainWithValue(GlobalKeyword.InheritValue);
    public FloatBuilder Initial => ChainWithValue(GlobalKeyword.InitialValue);
    public FloatBuilder Revert => ChainWithValue(GlobalKeyword.RevertValue);
    public FloatBuilder RevertLayer => ChainWithValue(GlobalKeyword.RevertLayerValue);
    public FloatBuilder Unset => ChainWithValue(GlobalKeyword.UnsetValue);

    public FloatBuilder OnPhone => ChainWithBreakpoint(Breakpoint.Phone);
    public FloatBuilder OnTablet => ChainWithBreakpoint(Breakpoint.Tablet);
    public FloatBuilder OnLaptop => ChainWithBreakpoint(Breakpoint.Laptop);
    public FloatBuilder OnDesktop => ChainWithBreakpoint(Breakpoint.Desktop);
    public FloatBuilder OnWidescreen => ChainWithBreakpoint(Breakpoint.Widescreen);
    public FloatBuilder OnUltrawide => ChainWithBreakpoint(Breakpoint.Ultrawide);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private FloatBuilder ChainWithValue(string value)
    {
        _rules.Add(new FloatRule(value, null));
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private FloatBuilder ChainWithBreakpoint(Breakpoint breakpoint)
    {
        if (_rules.Count == 0)
        {
            _rules.Add(new FloatRule(Float.NoneValue, breakpoint));
            return this;
        }

        int lastIdx = _rules.Count - 1;
        FloatRule last = _rules[lastIdx];
        _rules[lastIdx] = new FloatRule(last.Value, breakpoint);
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
            FloatRule rule = _rules[i];

            string cls = rule.Value switch
            {
                Float.LeftValue => _classStart,
                Float.RightValue => _classEnd,
                Float.InlineStartValue => _classStart,
                Float.InlineEndValue => _classEnd,
                Float.NoneValue => _classNone,
                _ => string.Empty
            };

            if (cls.Length == 0)
                continue;

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
            FloatRule rule = _rules[i];

            string? css = rule.Value switch
            {
                Float.LeftValue => "left",
                Float.RightValue => "right",
                Float.InlineStartValue => "inline-start",
                Float.InlineEndValue => "inline-end",
                Float.NoneValue => "none",
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

            sb.Append(_floatPrefix);
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
