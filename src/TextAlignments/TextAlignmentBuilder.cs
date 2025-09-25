using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Quark.Enums.Breakpoints;
using Soenneker.Utils.PooledStringBuilders;

namespace Soenneker.Quark;

public sealed class TextAlignmentBuilder : ICssBuilder
{
    private readonly List<TextAlignmentRule> _rules = new(4);

    // ----- Class name constants (compile-time) -----
    private const string _classStart = "text-start";
    private const string _classCenter = "text-center";
    private const string _classEnd = "text-end";

    // ----- CSS prefix (compile-time) -----
    private const string _textAlignPrefix = "text-align: ";

    // ----- Style constants (compile-time, Intellenum *Value are const) -----
    private const string _styleStart = $"{_textAlignPrefix}{Enums.TextAlignments.TextAlignment.StartValue}";
    private const string _styleCenter = $"{_textAlignPrefix}{Enums.TextAlignments.TextAlignment.CenterValue}";
    private const string _styleEnd = $"{_textAlignPrefix}{Enums.TextAlignments.TextAlignment.EndValue}";
    private const string _styleInherit = $"{_textAlignPrefix}{Enums.GlobalKeywords.GlobalKeyword.InheritValue}";
    private const string _styleInitial = $"{_textAlignPrefix}{Enums.GlobalKeywords.GlobalKeyword.InitialValue}";
    private const string _styleUnset = $"{_textAlignPrefix}{Enums.GlobalKeywords.GlobalKeyword.UnsetValue}";
    private const string _styleRevert = $"{_textAlignPrefix}{Enums.GlobalKeywords.GlobalKeyword.RevertValue}";
    private const string _styleRevertLayer = $"{_textAlignPrefix}{Enums.GlobalKeywords.GlobalKeyword.RevertLayerValue}";

    internal TextAlignmentBuilder(string value, Breakpoint? breakpoint = null)
    {
        _rules.Add(new TextAlignmentRule(value, breakpoint));
    }

    internal TextAlignmentBuilder(List<TextAlignmentRule> rules)
    {
        if (rules is { Count: > 0 })
            _rules.AddRange(rules);
    }

    public TextAlignmentBuilder Start => Chain(Enums.TextAlignments.TextAlignment.StartValue);
    public TextAlignmentBuilder Center => Chain(Enums.TextAlignments.TextAlignment.CenterValue);
    public TextAlignmentBuilder End => Chain(Enums.TextAlignments.TextAlignment.EndValue);

    public TextAlignmentBuilder Inherit => Chain(Enums.GlobalKeywords.GlobalKeyword.InheritValue);
    public TextAlignmentBuilder Initial => Chain(Enums.GlobalKeywords.GlobalKeyword.InitialValue);
    public TextAlignmentBuilder Revert => Chain(Enums.GlobalKeywords.GlobalKeyword.RevertValue);
    public TextAlignmentBuilder RevertLayer => Chain(Enums.GlobalKeywords.GlobalKeyword.RevertLayerValue);
    public TextAlignmentBuilder Unset => Chain(Enums.GlobalKeywords.GlobalKeyword.UnsetValue);

    public TextAlignmentBuilder OnPhone => ChainBp(Breakpoint.Phone);
    public TextAlignmentBuilder OnTablet => ChainBp(Breakpoint.Tablet);
    public TextAlignmentBuilder OnLaptop => ChainBp(Breakpoint.Laptop);
    public TextAlignmentBuilder OnDesktop => ChainBp(Breakpoint.Desktop);
    public TextAlignmentBuilder OnWidescreen => ChainBp(Breakpoint.Widescreen);
    public TextAlignmentBuilder OnUltrawide => ChainBp(Breakpoint.Ultrawide);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private TextAlignmentBuilder Chain(string value)
    {
        _rules.Add(new TextAlignmentRule(value, null));
        return this;
    }

    /// <summary>Apply a breakpoint to the most recent rule (or bootstrap with a default if empty).</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private TextAlignmentBuilder ChainBp(Breakpoint bp)
    {
        if (_rules.Count == 0)
        {
            _rules.Add(new TextAlignmentRule(Enums.TextAlignments.TextAlignment.StartValue, bp));
            return this;
        }

        int lastIdx = _rules.Count - 1;
        TextAlignmentRule last = _rules[lastIdx];
        _rules[lastIdx] = new TextAlignmentRule(last.Value, bp);
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
            TextAlignmentRule rule = _rules[i];

            string baseClass = rule.Value switch
            {
                Enums.TextAlignments.TextAlignment.StartValue => _classStart,
                Enums.TextAlignments.TextAlignment.CenterValue => _classCenter,
                Enums.TextAlignments.TextAlignment.EndValue => _classEnd,
                _ => string.Empty
            };

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

    public string ToStyle()
    {
        if (_rules.Count == 0)
            return string.Empty;

        using var sb = new PooledStringBuilder();
        var first = true;

        for (var i = 0; i < _rules.Count; i++)
        {
            TextAlignmentRule rule = _rules[i];

            string css = rule.Value switch
            {
                Enums.TextAlignments.TextAlignment.StartValue => _styleStart,
                Enums.TextAlignments.TextAlignment.CenterValue => _styleCenter,
                Enums.TextAlignments.TextAlignment.EndValue => _styleEnd,

                Enums.GlobalKeywords.GlobalKeyword.InheritValue => _styleInherit,
                Enums.GlobalKeywords.GlobalKeyword.InitialValue => _styleInitial,
                Enums.GlobalKeywords.GlobalKeyword.UnsetValue => _styleUnset,
                Enums.GlobalKeywords.GlobalKeyword.RevertValue => _styleRevert,
                Enums.GlobalKeywords.GlobalKeyword.RevertLayerValue => _styleRevertLayer,

                _ => string.Empty
            };

            if (css.Length == 0)
                continue;

            if (!first)
                sb.Append("; ");
            else
                first = false;

            sb.Append(css);
        }

        return sb.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetBp(Breakpoint? breakpoint)
    {
        return breakpoint?.Value ?? string.Empty;
    }

    /// <summary>
    /// Insert breakpoint token as: "text-center" + "md" ? "text-md-center".
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
