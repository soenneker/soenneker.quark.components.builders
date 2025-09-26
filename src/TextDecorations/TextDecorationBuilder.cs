using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Utils.PooledStringBuilders;

namespace Soenneker.Quark;

public sealed class TextDecorationBuilder : ICssBuilder
{
    private readonly List<TextDecorationRule> _rules = new(4);

    // ----- Class name constants (compile-time) -----
    private const string _classNone = "text-decoration-none";
    private const string _classUnderline = "text-decoration-underline";
    private const string _classLineThrough = "text-decoration-line-through";

    // ----- CSS value prefix (compile-time) -----
    private const string _textDecorationPrefix = "text-decoration-line: ";

    // ----- Style constants (compile-time const interpolation with Intellenum consts) -----
    private const string _styleNone = $"{_textDecorationPrefix}{Enums.TextDecorations.Line.TextDecorationLine.NoneValue}";
    private const string _styleUnderline = $"{_textDecorationPrefix}{Enums.TextDecorations.Line.TextDecorationLine.UnderlineValue}";
    private const string _styleLineThrough = $"{_textDecorationPrefix}{Enums.TextDecorations.Line.TextDecorationLine.LineThroughValue}";

    private const string _styleInherit = $"{_textDecorationPrefix}{Enums.GlobalKeywords.GlobalKeyword.InheritValue}";
    private const string _styleInitial = $"{_textDecorationPrefix}{Enums.GlobalKeywords.GlobalKeyword.InitialValue}";
    private const string _styleUnset = $"{_textDecorationPrefix}{Enums.GlobalKeywords.GlobalKeyword.UnsetValue}";
    private const string _styleRevert = $"{_textDecorationPrefix}{Enums.GlobalKeywords.GlobalKeyword.RevertValue}";
    private const string _styleRevertLayer = $"{_textDecorationPrefix}{Enums.GlobalKeywords.GlobalKeyword.RevertLayerValue}";

    internal TextDecorationBuilder(string value, Breakpoint? breakpoint = null)
    {
        _rules.Add(new TextDecorationRule(value, breakpoint));
    }

    internal TextDecorationBuilder(List<TextDecorationRule> rules)
    {
        if (rules is { Count: > 0 })
            _rules.AddRange(rules);
    }

    public TextDecorationBuilder None => Chain(Enums.TextDecorations.Line.TextDecorationLine.NoneValue);
    public TextDecorationBuilder Underline => Chain(Enums.TextDecorations.Line.TextDecorationLine.UnderlineValue);
    public TextDecorationBuilder LineThrough => Chain(Enums.TextDecorations.Line.TextDecorationLine.LineThroughValue);

    public TextDecorationBuilder Inherit => Chain(Enums.GlobalKeywords.GlobalKeyword.InheritValue);
    public TextDecorationBuilder Initial => Chain(Enums.GlobalKeywords.GlobalKeyword.InitialValue);
    public TextDecorationBuilder Revert => Chain(Enums.GlobalKeywords.GlobalKeyword.RevertValue);
    public TextDecorationBuilder RevertLayer => Chain(Enums.GlobalKeywords.GlobalKeyword.RevertLayerValue);
    public TextDecorationBuilder Unset => Chain(Enums.GlobalKeywords.GlobalKeyword.UnsetValue);

    public TextDecorationBuilder OnPhone => ChainBp(Breakpoint.Phone);
    public TextDecorationBuilder OnTablet => ChainBp(Breakpoint.Tablet);
    public TextDecorationBuilder OnLaptop => ChainBp(Breakpoint.Laptop);
    public TextDecorationBuilder OnDesktop => ChainBp(Breakpoint.Desktop);
    public TextDecorationBuilder OnWidescreen => ChainBp(Breakpoint.Widescreen);
    public TextDecorationBuilder OnUltrawide => ChainBp(Breakpoint.Ultrawide);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private TextDecorationBuilder Chain(string value)
    {
        _rules.Add(new TextDecorationRule(value, null));
        return this;
    }

    /// <summary>Apply a breakpoint to the most recent rule (or bootstrap with a default if empty).</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private TextDecorationBuilder ChainBp(Breakpoint bp)
    {
        if (_rules.Count == 0)
        {
            _rules.Add(new TextDecorationRule(Enums.TextDecorations.Line.TextDecorationLine.NoneValue, bp));
            return this;
        }

        int lastIdx = _rules.Count - 1;
        TextDecorationRule last = _rules[lastIdx];
        _rules[lastIdx] = new TextDecorationRule(last.Value, bp);
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
            TextDecorationRule rule = _rules[i];

            string baseClass = rule.Value switch
            {
                Enums.TextDecorations.Line.TextDecorationLine.NoneValue => _classNone,
                Enums.TextDecorations.Line.TextDecorationLine.UnderlineValue => _classUnderline,
                Enums.TextDecorations.Line.TextDecorationLine.LineThroughValue => _classLineThrough,
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
            TextDecorationRule rule = _rules[i];

            string css = rule.Value switch
            {
                Enums.TextDecorations.Line.TextDecorationLine.NoneValue => _styleNone,
                Enums.TextDecorations.Line.TextDecorationLine.UnderlineValue => _styleUnderline,
                Enums.TextDecorations.Line.TextDecorationLine.LineThroughValue => _styleLineThrough,

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
    private static string GetBp(Breakpoint? breakpoint) => breakpoint?.Value ?? string.Empty;

    /// <summary>
    /// Insert breakpoint token as: "text-decoration-underline" + "md" ? "text-decoration-md-underline".
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
