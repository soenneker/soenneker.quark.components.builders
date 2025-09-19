using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Quark.Components.Builders.Abstract;
using Soenneker.Quark.Components.Builders.Utils;
using Soenneker.Quark.Enums.Breakpoints;
using Soenneker.Quark.Enums.Size;
using Soenneker.Utils.PooledStringBuilders;

namespace Soenneker.Quark.Components.Builders.TextSizes;

/// <summary>
/// High-performance text size builder with fluent API for chaining text size rules.
/// </summary>
public sealed class TextSizeBuilder : ICssBuilder
{
    private readonly List<TextSizeRule> _rules = new(4);

    // ----- Class name constants -----
    private const string _fs6 = "fs-6";
    private const string _fs5 = "fs-5";
    private const string _fs4 = "fs-4";
    private const string _fs3 = "fs-3";
    private const string _fs2 = "fs-2";
    private const string _fs1 = "fs-1";
    private const string _display6 = "display-6";
    private const string _display5 = "display-5";
    private const string _display4 = "display-4";
    private const string _display3 = "display-3";
    private const string _display2 = "display-2";
    private const string _display1 = "display-1";

    // ----- CSS prefix (compile-time) -----
    private const string _fontSizePrefix = "font-size: ";

    internal TextSizeBuilder(string size, Breakpoint? breakpoint = null)
    {
        _rules.Add(new TextSizeRule(size, breakpoint));
    }

    internal TextSizeBuilder(List<TextSizeRule> rules)
    {
        if (rules is { Count: > 0 })
            _rules.AddRange(rules);
    }

    // ----- Fluent size chaining -----
    public TextSizeBuilder Xs => ChainSize(Size.ExtraSmall.Value);
    public TextSizeBuilder Sm => ChainSize(Size.Small.Value);
    public TextSizeBuilder Base => ChainSize("base");
    public TextSizeBuilder Lg => ChainSize(Size.Large.Value);
    public TextSizeBuilder Xl => ChainSize(Size.ExtraLarge.Value);
    public TextSizeBuilder Xl2 => ChainSize("2xl");
    public TextSizeBuilder Xl3 => ChainSize("3xl");
    public TextSizeBuilder Xl4 => ChainSize("4xl");
    public TextSizeBuilder Xl5 => ChainSize("5xl");
    public TextSizeBuilder Xl6 => ChainSize("6xl");
    public TextSizeBuilder Xl7 => ChainSize("7xl");
    public TextSizeBuilder Xl8 => ChainSize("8xl");
    public TextSizeBuilder Xl9 => ChainSize("9xl");

    // ----- Breakpoint chaining -----
    public TextSizeBuilder OnPhone => ChainBp(Breakpoint.Phone);
    public TextSizeBuilder OnTablet => ChainBp(Breakpoint.Tablet);
    public TextSizeBuilder OnLaptop => ChainBp(Breakpoint.Laptop);
    public TextSizeBuilder OnDesktop => ChainBp(Breakpoint.Desktop);
    public TextSizeBuilder OnWidescreen => ChainBp(Breakpoint.Widescreen);
    public TextSizeBuilder OnUltrawide => ChainBp(Breakpoint.Ultrawide);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private TextSizeBuilder ChainSize(string size)
    {
        _rules.Add(new TextSizeRule(size, null));
        return this;
    }

    /// <summary>Apply a breakpoint to the most recent rule (or bootstrap with "base" if empty).</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private TextSizeBuilder ChainBp(Breakpoint bp)
    {
        if (_rules.Count == 0)
        {
            _rules.Add(new TextSizeRule("base", bp));
            return this;
        }

        int lastIdx = _rules.Count - 1;
        TextSizeRule last = _rules[lastIdx];
        _rules[lastIdx] = new TextSizeRule(last.Size, bp);
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
            TextSizeRule rule = _rules[i];

            string sizeClass = GetSizeClass(rule.Size);
            if (sizeClass.Length == 0)
                continue;

            string bp = BreakpointUtil.GetBreakpointClass(rule.Breakpoint);
            if (bp.Length != 0)
                sizeClass = InsertBreakpoint(sizeClass, bp);

            if (!first)
                sb.Append(' ');
            else
                first = false;

            sb.Append(sizeClass);
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
            TextSizeRule rule = _rules[i];

            string? sizeValue = GetSizeValue(rule.Size);
            if (sizeValue is null)
                continue;

            if (!first)
                sb.Append("; ");
            else
                first = false;

            sb.Append(_fontSizePrefix);
            sb.Append(sizeValue);
        }

        return sb.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetSizeClass(string size)
    {
        // Bootstrap 5 font-scale mapping
        return size switch
        {
            "xs" => _fs6,
            "sm" => _fs5,
            "base" => _fs4,
            "lg" => _fs3,
            "xl" => _fs2,
            "2xl" => _fs1,

            "3xl" => _display6,
            "4xl" => _display5,
            "5xl" => _display4,
            "6xl" => _display3,
            "7xl" => _display2,
            "8xl" => _display1,
            "9xl" => _display1, // no display-0; clamp
            _ => string.Empty
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string? GetSizeValue(string size)
    {
        // Tailored inline values for when you need styles instead of classes
        return size switch
        {
            "xs" => "0.75rem",
            "sm" => "0.875rem",
            "base" => "1rem",
            "lg" => "1.125rem",
            "xl" => "1.25rem",
            "2xl" => "1.5rem",
            "3xl" => "1.75rem",
            "4xl" => "2rem",
            "5xl" => "2.25rem",
            "6xl" => "2.5rem",
            "7xl" => "3rem",
            "8xl" => "3.5rem",
            "9xl" => "4rem",
            _ => null
        };
    }


    /// <summary>
    /// Insert breakpoint token as: "fs-4" + "md" → "fs-md-4".
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
