using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Quark.Enums;
using Soenneker.Utils.PooledStringBuilders;

namespace Soenneker.Quark;

public sealed class FontWeightBuilder : ICssBuilder
{
    private readonly List<FontWeightRule> _rules = new(6);

    private const string _classLight = "fw-light";
    private const string _classNormal = "fw-normal";
    private const string _classMedium = "fw-medium";
    private const string _classSemibold = "fw-semibold";
    private const string _classBold = "fw-bold";
    private const string _classBolder = "fw-bolder";
    private const string _stylePrefix = "font-weight: ";

    internal FontWeightBuilder(string value, Breakpoint? breakpoint = null)
    {
        _rules.Add(new FontWeightRule(value, breakpoint));
    }

    internal FontWeightBuilder(List<FontWeightRule> rules)
    {
        if (rules is { Count: > 0 })
            _rules.AddRange(rules);
    }

    public FontWeightBuilder Light => Chain(Enums.FontWeight.LightValue);
    public FontWeightBuilder Normal => Chain(Enums.FontWeight.NormalValue);
    public FontWeightBuilder Medium => Chain(Enums.FontWeight.MediumValue);
    public FontWeightBuilder Semibold => Chain(Enums.FontWeight.SemiboldValue);
    public FontWeightBuilder Bold => Chain(Enums.FontWeight.BoldValue);
    public FontWeightBuilder Bolder => Chain(Enums.FontWeight.BolderValue);
    public FontWeightBuilder Inherit => Chain(GlobalKeyword.InheritValue);
    public FontWeightBuilder Initial => Chain(GlobalKeyword.InitialValue);
    public FontWeightBuilder Revert => Chain(GlobalKeyword.RevertValue);
    public FontWeightBuilder RevertLayer => Chain(GlobalKeyword.RevertLayerValue);
    public FontWeightBuilder Unset => Chain(GlobalKeyword.UnsetValue);

    public FontWeightBuilder OnPhone => ChainBp(Breakpoint.Phone);

    public FontWeightBuilder OnTablet => ChainBp(Breakpoint.Tablet);

    public FontWeightBuilder OnLaptop => ChainBp(Breakpoint.Laptop);

    public FontWeightBuilder OnDesktop => ChainBp(Breakpoint.Desktop);

    public FontWeightBuilder OnWideScreen => ChainBp(Breakpoint.Widescreen);

    public FontWeightBuilder OnUltrawide => ChainBp(Breakpoint.Ultrawide);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private FontWeightBuilder Chain(string value)
    {
        _rules.Add(new FontWeightRule(value, null));
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private FontWeightBuilder ChainBp(Breakpoint bp)
    {
        if (_rules.Count == 0)
        {
            _rules.Add(new FontWeightRule(Enums.FontWeight.NormalValue, bp));
            return this;
        }

        int lastIdx = _rules.Count - 1;
        FontWeightRule last = _rules[lastIdx];
        _rules[lastIdx] = new FontWeightRule(last.Value, bp);
        return this;
    }

    public string ToClass()
    {
        if (_rules.Count == 0) return string.Empty;

        using var sb = new PooledStringBuilder();
        var first = true;
        for (var i = 0; i < _rules.Count; i++)
        {
            FontWeightRule rule = _rules[i];
            string cls = rule.Value switch
            {
                Enums.FontWeight.LightValue => _classLight,
                Enums.FontWeight.NormalValue => _classNormal,
                Enums.FontWeight.MediumValue => _classMedium,
                Enums.FontWeight.SemiboldValue => _classSemibold,
                Enums.FontWeight.BoldValue => _classBold,
                Enums.FontWeight.BolderValue => _classBolder,
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
            FontWeightRule rule = _rules[i];
            string? css = rule.Value switch
            {
                Enums.FontWeight.LightValue => "300",
                Enums.FontWeight.NormalValue => "400",
                Enums.FontWeight.MediumValue => "500",
                Enums.FontWeight.SemiboldValue => "600",
                Enums.FontWeight.BoldValue => "700",
                Enums.FontWeight.BolderValue => "bolder",
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

            sb.Append(_stylePrefix);
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
