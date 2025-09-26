using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Utils.PooledStringBuilders;

namespace Soenneker.Quark;

public sealed class FontStyleBuilder : ICssBuilder
{
    private readonly List<FontStyleRule> _rules = new(4);

    private const string _classItalic = "fst-italic";
    private const string _classNormal = "fst-normal";
    private const string _stylePrefix = "font-style: ";

    internal FontStyleBuilder(string value, Breakpoint? breakpoint = null)
    {
        _rules.Add(new FontStyleRule(value, breakpoint));
    }

    internal FontStyleBuilder(List<FontStyleRule> rules)
    {
        if (rules is { Count: > 0 })
            _rules.AddRange(rules);
    }

    public FontStyleBuilder Italic => Chain(Enums.FontStyles.FontStyle.ItalicValue);
    public FontStyleBuilder Normal => Chain(Enums.FontStyles.FontStyle.NormalValue);
    public FontStyleBuilder Inherit => Chain(Enums.GlobalKeywords.GlobalKeyword.InheritValue);
    public FontStyleBuilder Initial => Chain(Enums.GlobalKeywords.GlobalKeyword.InitialValue);
    public FontStyleBuilder Revert => Chain(Enums.GlobalKeywords.GlobalKeyword.RevertValue);
    public FontStyleBuilder RevertLayer => Chain(Enums.GlobalKeywords.GlobalKeyword.RevertLayerValue);
    public FontStyleBuilder Unset => Chain(Enums.GlobalKeywords.GlobalKeyword.UnsetValue);

    public FontStyleBuilder OnPhone => ChainBp(Breakpoint.Phone);
    public FontStyleBuilder OnTablet => ChainBp(Breakpoint.Tablet);
    public FontStyleBuilder OnLaptop => ChainBp(Breakpoint.Laptop);
    public FontStyleBuilder OnDesktop => ChainBp(Breakpoint.Desktop);
    public FontStyleBuilder OnWidescreen => ChainBp(Breakpoint.Widescreen);
    public FontStyleBuilder OnUltrawide => ChainBp(Breakpoint.Ultrawide);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private FontStyleBuilder Chain(string value)
    {
        _rules.Add(new FontStyleRule(value, null));
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private FontStyleBuilder ChainBp(Breakpoint bp)
    {
        if (_rules.Count == 0)
        {
            _rules.Add(new FontStyleRule(Enums.FontStyles.FontStyle.NormalValue, bp));
            return this;
        }

        int lastIdx = _rules.Count - 1;
        FontStyleRule last = _rules[lastIdx];
        _rules[lastIdx] = new FontStyleRule(last.Value, bp);
        return this;
    }

    public string ToClass()
    {
        if (_rules.Count == 0) return string.Empty;

        using var sb = new PooledStringBuilder();
        var first = true;
        for (var i = 0; i < _rules.Count; i++)
        {
            FontStyleRule rule = _rules[i];
            string cls = rule.Value switch
            {
                Enums.FontStyles.FontStyle.ItalicValue => _classItalic,
                Enums.FontStyles.FontStyle.NormalValue => _classNormal,
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
            FontStyleRule rule = _rules[i];
            string val = rule.Value;
            if (string.IsNullOrEmpty(val))
                continue;

            if (!first) sb.Append("; ");
            else first = false;

            sb.Append(_stylePrefix);
            sb.Append(val);
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

 
