using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Quark.Enums;
using Soenneker.Utils.PooledStringBuilders;

namespace Soenneker.Quark;

/// <summary>
/// Simplified gap builder with fluent API for chaining gap rules.
/// </summary>
public sealed class GapBuilder : ICssBuilder
{
    private readonly List<GapRule> _rules = new(4);

    private const string _classGap0 = "gap-0";
    private const string _classGap1 = "gap-1";
    private const string _classGap2 = "gap-2";
    private const string _classGap3 = "gap-3";
    private const string _classGap4 = "gap-4";
    private const string _classGap5 = "gap-5";
    private const string _stylePrefix = "gap: ";

    internal GapBuilder(string size, Breakpoint? breakpoint = null)
    {
        if (!string.IsNullOrEmpty(size))
            _rules.Add(new GapRule(size, breakpoint));
    }

    internal GapBuilder(List<GapRule> rules)
    {
        if (rules is { Count: > 0 })
            _rules.AddRange(rules);
    }

    /// <summary>
    /// Chain with a new size for the next rule.
    /// </summary>
    public GapBuilder S0 => ChainWithSize(ScaleType.S0Value);

    /// <summary>
    /// Chain with a new size for the next rule.
    /// </summary>
    public GapBuilder S1 => ChainWithSize(ScaleType.S1Value);

    /// <summary>
    /// Chain with a new size for the next rule.
    /// </summary>
    public GapBuilder S2 => ChainWithSize(ScaleType.S2Value);

    /// <summary>
    /// Chain with a new size for the next rule.
    /// </summary>
    public GapBuilder S3 => ChainWithSize(ScaleType.S3Value);

    /// <summary>
    /// Chain with a new size for the next rule.
    /// </summary>
    public GapBuilder S4 => ChainWithSize(ScaleType.S4Value);

    /// <summary>
    /// Chain with a new size for the next rule.
    /// </summary>
    public GapBuilder S5 => ChainWithSize(ScaleType.S5Value);

    /// <summary>
    /// Apply on phone devices (portrait phones, less than 576px).
    /// </summary>
    public GapBuilder OnPhone => ChainWithBreakpoint(Breakpoint.Phone);

    /// <summary>
    /// Apply on mobile devices (landscape phones, 576px and up).
    /// </summary>
    public GapBuilder OnTablet => ChainWithBreakpoint(Breakpoint.Tablet);

    /// <summary>
    /// Apply on laptop devices (laptops, 992px and up).
    /// </summary>
    public GapBuilder OnLaptop => ChainWithBreakpoint(Breakpoint.Laptop);

    /// <summary>
    /// Apply on desktop devices (desktops, 1200px and up).
    /// </summary>
    public GapBuilder OnDesktop => ChainWithBreakpoint(Breakpoint.Desktop);

    /// <summary>
    /// Apply on wide screen devices (larger desktops, 1400px and up).
    /// </summary>
    public GapBuilder OnWidescreen => ChainWithBreakpoint(Breakpoint.Widescreen);
    public GapBuilder OnUltrawide => ChainWithBreakpoint(Breakpoint.Ultrawide);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private GapBuilder ChainWithSize(string size)
    {
        _rules.Add(new GapRule(size, null));
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private GapBuilder ChainWithBreakpoint(Breakpoint breakpoint)
    {
        if (_rules.Count == 0)
        {
            _rules.Add(new GapRule(ScaleType.S0Value, breakpoint));
            return this;
        }

        int lastIdx = _rules.Count - 1;
        GapRule last = _rules[lastIdx];
        _rules[lastIdx] = new GapRule(last.Size, breakpoint);
        return this;
    }

    /// <summary>
    /// Gets the CSS class string for the current configuration.
    /// </summary>
    public string ToClass()
    {
        if (_rules.Count == 0)
            return string.Empty;

        using var sb = new PooledStringBuilder();
        var first = true;

        for (var i = 0; i < _rules.Count; i++)
        {
            GapRule rule = _rules[i];
            string cls = GetSizeClass(rule.Size);
            if (cls.Length == 0)
                continue;

            string bp = BreakpointUtil.GetBreakpointClass(rule.Breakpoint);

            if (bp.Length != 0)
                cls = InsertBreakpoint(cls, bp);

            if (!first) 
                sb.Append(' ');
            else first = false;

            sb.Append(cls);
        }

        return sb.ToString();
    }

    /// <summary>
    /// Gets the CSS style string for the current configuration.
    /// </summary>
    public string ToStyle()
    {
        if (_rules.Count == 0)
            return string.Empty;

        using var sb = new PooledStringBuilder();
        var first = true;

        for (var i = 0; i < _rules.Count; i++)
        {
            GapRule rule = _rules[i];
            string? sizeValue = GetSizeValue(rule.Size);
            if (sizeValue is null)
                continue;

            if (!first) sb.Append("; ");
            else first = false;

            sb.Append(_stylePrefix);
            sb.Append(sizeValue);
        }

        return sb.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetSizeClass(string size)
    {
        return size switch
        {
            ScaleType.S0Value => _classGap0,
            ScaleType.S1Value => _classGap1,
            ScaleType.S2Value => _classGap2,
            ScaleType.S3Value => _classGap3,
            ScaleType.S4Value => _classGap4,
            ScaleType.S5Value => _classGap5,
            _ => string.Empty
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string? GetSizeValue(string size)
    {
        return size switch
        {
            ScaleType.S0Value => "0",
            ScaleType.S1Value => "0.25rem",
            ScaleType.S2Value => "0.5rem",
            ScaleType.S3Value => "1rem",
            ScaleType.S4Value => "1.5rem",
            ScaleType.S5Value => "3rem",
            _ => null
        };
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
