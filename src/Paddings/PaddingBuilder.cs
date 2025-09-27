using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Quark.Enums;
using Soenneker.Utils.PooledStringBuilders;

namespace Soenneker.Quark;

/// <summary>
/// High-performance padding builder with fluent API for chaining padding rules.
/// </summary>
public sealed class PaddingBuilder : ICssBuilder
{
    private readonly List<PaddingRule> _rules = new(4);

    // ----- Class tokens -----
    private const string _baseToken = "p";

    // ----- Size tokens -----
    private const string _token0 = "0";
    private const string _tokenAuto = "auto";

    // ----- Side tokens (Bootstrap naming) -----
    private const string _sideT = "t";
    private const string _sideE = "e";
    private const string _sideB = "b";
    private const string _sideS = "s";
    private const string _sideX = "x";
    private const string _sideY = "y";

    internal PaddingBuilder(string size, Breakpoint? breakpoint = null)
    {
        _rules.Add(new PaddingRule(size, ElementSideType.All, breakpoint));
    }

    internal PaddingBuilder(List<PaddingRule> rules)
    {
        if (rules is { Count: > 0 })
            _rules.AddRange(rules);
    }

    // ----- Side chaining -----
    public PaddingBuilder FromTop => AddRule(ElementSideType.Top);
    public PaddingBuilder FromRight => AddRule(ElementSideType.Right);
    public PaddingBuilder FromBottom => AddRule(ElementSideType.Bottom);
    public PaddingBuilder FromLeft => AddRule(ElementSideType.Left);
    public PaddingBuilder OnX => AddRule(ElementSideType.Horizontal);
    public PaddingBuilder OnY => AddRule(ElementSideType.Vertical);
    public PaddingBuilder OnAll => AddRule(ElementSideType.All);
    public PaddingBuilder FromStart => AddRule(ElementSideType.InlineStart);
    public PaddingBuilder FromEnd => AddRule(ElementSideType.InlineEnd);

    // ----- Size chaining -----
    public PaddingBuilder S0 => ChainWithSize(ScaleType.S0);
    public PaddingBuilder S1 => ChainWithSize(ScaleType.S1);
    public PaddingBuilder S2 => ChainWithSize(ScaleType.S2);
    public PaddingBuilder S3 => ChainWithSize(ScaleType.S3);
    public PaddingBuilder S4 => ChainWithSize(ScaleType.S4);
    public PaddingBuilder S5 => ChainWithSize(ScaleType.S5);

    // ----- Breakpoint chaining -----
    public PaddingBuilder OnPhone => ChainWithBreakpoint(Breakpoint.Phone);
    public PaddingBuilder OnTablet => ChainWithBreakpoint(Breakpoint.Tablet);
    public PaddingBuilder OnLaptop => ChainWithBreakpoint(Breakpoint.Laptop);
    public PaddingBuilder OnDesktop => ChainWithBreakpoint(Breakpoint.Desktop);
    public PaddingBuilder OnWidescreen => ChainWithBreakpoint(Breakpoint.Widescreen);
    public PaddingBuilder OnUltrawide => ChainWithBreakpoint(Breakpoint.Ultrawide);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private PaddingBuilder AddRule(ElementSideType side)
    {
        // Use last size & breakpoint if present; default to ScaleType.S0Value when absent
        string size = _rules.Count > 0 ? _rules[^1].Size : ScaleType.S0Value;
        Breakpoint? bp = _rules.Count > 0 ? _rules[^1].Breakpoint : null;

        if (_rules.Count > 0 && _rules[^1].Side == ElementSideType.All)
        {
            // Replace last "All" with specific side using same size/bp
            _rules[^1] = new PaddingRule(size, side, bp);
        }
        else
        {
            _rules.Add(new PaddingRule(size, side, bp));
        }

        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private PaddingBuilder ChainWithSize(string size)
    {
        _rules.Add(new PaddingRule(size, ElementSideType.All, null));
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private PaddingBuilder ChainWithSize(ScaleType scale)
    {
        _rules.Add(new PaddingRule(scale.Value, ElementSideType.All, null));
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private PaddingBuilder ChainWithBreakpoint(Breakpoint breakpoint)
    {
        if (_rules.Count == 0)
        {
            _rules.Add(new PaddingRule(ScaleType.S0Value, ElementSideType.All, breakpoint));
            return this;
        }

        int lastIdx = _rules.Count - 1;
        PaddingRule last = _rules[lastIdx];
        _rules[lastIdx] = new PaddingRule(last.Size, last.Side, breakpoint);
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
            PaddingRule rule = _rules[i];

            string sizeTok = GetSizeToken(rule.Size);
            if (sizeTok.Length == 0)
                continue;

            string sideTok = GetSideToken(rule.Side); // "", "t", "e", "b", "s", "x", "y"
            string bpTok = BreakpointUtil.GetBreakpointToken(rule.Breakpoint); // "", "sm", "md", ...

            if (!first) sb.Append(' ');
            else first = false;

            // Build: p{side?}-{bp?}-{size}
            sb.Append(_baseToken);
            if (sideTok.Length != 0)
                sb.Append(sideTok);

            sb.Append('-');

            if (bpTok.Length != 0)
            {
                sb.Append(bpTok);
                sb.Append('-');
            }

            sb.Append(sizeTok);
        }

        return sb.ToString();
    }

    /// <summary>Gets the CSS style string for the current configuration.</summary>
    public string ToStyle()
    {
        if (_rules.Count == 0)
            return string.Empty;

        var sb = new PooledStringBuilder();
        var first = true;

        try
        {
            for (var i = 0; i < _rules.Count; i++)
            {
                PaddingRule rule = _rules[i];
                string? sizeVal = GetSizeValue(rule.Size);
                if (sizeVal is null)
                    continue;

                switch (rule.Side)
                {
                    case ElementSideType.AllValue:
                        AppendStyle(ref first, ref sb, "padding", sizeVal);
                        break;

                    case ElementSideType.TopValue:
                        AppendStyle(ref first, ref sb, "padding-top", sizeVal);
                        break;

                    case ElementSideType.RightValue:
                        AppendStyle(ref first, ref sb, "padding-right", sizeVal);
                        break;

                    case ElementSideType.BottomValue:
                        AppendStyle(ref first, ref sb, "padding-bottom", sizeVal);
                        break;

                    case ElementSideType.LeftValue:
                        AppendStyle(ref first, ref sb, "padding-left", sizeVal);
                        break;

                    case ElementSideType.HorizontalValue:
                    case ElementSideType.LeftRightValue:
                        AppendStyle(ref first, ref sb, "padding-left", sizeVal);
                        AppendStyle(ref first, ref sb, "padding-right", sizeVal);
                        break;

                    case ElementSideType.VerticalValue:
                    case ElementSideType.TopBottomValue:
                        AppendStyle(ref first, ref sb, "padding-top", sizeVal);
                        AppendStyle(ref first, ref sb, "padding-bottom", sizeVal);
                        break;

                    case ElementSideType.InlineStartValue:
                        AppendStyle(ref first, ref sb, "padding-inline-start", sizeVal);
                        break;

                    case ElementSideType.InlineEndValue:
                        AppendStyle(ref first, ref sb, "padding-inline-end", sizeVal);
                        break;

                    default:
                        // Fallback like "all"
                        AppendStyle(ref first, ref sb, "padding", sizeVal);
                        break;
                }
            }

            return sb.ToString();
        }
        finally
        {
            sb.Dispose();
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void AppendStyle(ref bool first, ref PooledStringBuilder sb, string prop, string val)
    {
        if (!first) sb.Append("; ");
        else first = false;

        sb.Append(prop);
        sb.Append(": ");
        sb.Append(val);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetSizeToken(string size)
    {
        return size switch
        {
            ScaleType.S0Value => _token0,
            ScaleType.S1Value => ScaleType.S1Value,
            ScaleType.S2Value => ScaleType.S2Value,
            ScaleType.S3Value => ScaleType.S3Value,
            ScaleType.S4Value => ScaleType.S4Value,
            ScaleType.S5Value => ScaleType.S5Value,
            "-1" => _tokenAuto, // "auto"
            _ => string.Empty
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetSideToken(ElementSideType side)
    {
        switch (side)
        {
            case ElementSideType.AllValue:
                return string.Empty;
            case ElementSideType.TopValue:
                return _sideT;
            case ElementSideType.RightValue:
                return _sideE;
            case ElementSideType.BottomValue:
                return _sideB;
            case ElementSideType.LeftValue:
                return _sideS;
            case ElementSideType.HorizontalValue:
            case ElementSideType.LeftRightValue:
                return _sideX;
            case ElementSideType.VerticalValue:
            case ElementSideType.TopBottomValue:
                return _sideY;
            case ElementSideType.InlineStartValue:
                return _sideS; // Bootstrap uses "s" for start
            case ElementSideType.InlineEndValue:
                return _sideE; // Bootstrap uses "e" for end
            default:
                return string.Empty;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string? GetSizeValue(string size)
    {
        // Match your original rem scale and "auto"
        return size switch
        {
            ScaleType.S0Value => "0",
            ScaleType.S1Value => "0.25rem",
            ScaleType.S2Value => "0.5rem",
            ScaleType.S3Value => "1rem",
            ScaleType.S4Value => "1.5rem",
            ScaleType.S5Value => "3rem",
            "-1" => "auto",
            _ => null
        };
    }

}
