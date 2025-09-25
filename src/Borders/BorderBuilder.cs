using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Quark.Enums.Breakpoints;
using Soenneker.Quark.Enums.ElementSides;
using Soenneker.Quark.Enums.Scales;
using Soenneker.Utils.PooledStringBuilders;

namespace Soenneker.Quark;

/// <summary>
/// Simplified border builder with fluent API for chaining border rules.
/// </summary>
public sealed class BorderBuilder : ICssBuilder
{
    private readonly List<BorderRule> _rules = new(4);

    // ----- Class tokens -----
    private const string _baseToken = "b";

    // ----- Side tokens -----
    private const string _sideT = "t";
    private const string _sideE = "e";
    private const string _sideB = "b";
    private const string _sideS = "s";
    private const string _sideX = "x";
    private const string _sideY = "y";

    internal BorderBuilder(string size, Breakpoint? breakpoint = null)
    {
        if (!string.IsNullOrEmpty(size))
            _rules.Add(new BorderRule(size, ElementSide.All, breakpoint));
    }

    internal BorderBuilder(List<BorderRule> rules)
    {
        if (rules is { Count: > 0 })
            _rules.AddRange(rules);
    }

    // ----- Side chaining -----
    public BorderBuilder FromTop => AddRule(ElementSide.Top);
    public BorderBuilder FromRight => AddRule(ElementSide.Right);
    public BorderBuilder FromBottom => AddRule(ElementSide.Bottom);
    public BorderBuilder FromLeft => AddRule(ElementSide.Left);
    public BorderBuilder OnX => AddRule(ElementSide.Horizontal);
    public BorderBuilder OnY => AddRule(ElementSide.Vertical);
    public BorderBuilder OnAll => AddRule(ElementSide.All);
    public BorderBuilder FromStart => AddRule(ElementSide.InlineStart);
    public BorderBuilder FromEnd => AddRule(ElementSide.InlineEnd);

    // ----- Size chaining -----
    public BorderBuilder S0 => ChainWithSize(Scale.S0);
    public BorderBuilder S1 => ChainWithSize(Scale.S1);
    public BorderBuilder S2 => ChainWithSize(Scale.S2);
    public BorderBuilder S3 => ChainWithSize(Scale.S3);
    public BorderBuilder S4 => ChainWithSize(Scale.S4);
    public BorderBuilder S5 => ChainWithSize(Scale.S5);

    // ----- Breakpoint chaining -----
    public BorderBuilder OnPhone => ChainWithBreakpoint(Breakpoint.Phone);
    public BorderBuilder OnTablet => ChainWithBreakpoint(Breakpoint.Tablet);
    public BorderBuilder OnLaptop => ChainWithBreakpoint(Breakpoint.Laptop);
    public BorderBuilder OnDesktop => ChainWithBreakpoint(Breakpoint.Desktop);
    public BorderBuilder OnWidescreen => ChainWithBreakpoint(Breakpoint.Widescreen);
    public BorderBuilder OnUltrawide => ChainWithBreakpoint(Breakpoint.Ultrawide);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private BorderBuilder AddRule(ElementSide side)
    {
        string size = _rules.Count > 0 ? _rules[^1].Size : "0";
        Breakpoint? bp = _rules.Count > 0 ? _rules[^1].Breakpoint : null;

        if (_rules.Count > 0 && _rules[^1].Side == ElementSide.All)
        {
            _rules[^1] = new BorderRule(size, side, bp);
        }
        else
        {
            _rules.Add(new BorderRule(size, side, bp));
        }

        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private BorderBuilder ChainWithSize(Scale scale)
    {
        _rules.Add(new BorderRule(scale.Value, ElementSide.All, null));
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private BorderBuilder ChainWithBreakpoint(Breakpoint breakpoint)
    {
        if (_rules.Count == 0)
        {
            _rules.Add(new BorderRule("0", ElementSide.All, breakpoint));
            return this;
        }

        int lastIdx = _rules.Count - 1;
        BorderRule last = _rules[lastIdx];
        _rules[lastIdx] = new BorderRule(last.Size, last.Side, breakpoint);
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
            BorderRule rule = _rules[i];

            string sizeTok = rule.Size;

            if (sizeTok.Length == 0)
                continue;

            string sideTok = GetSideToken(rule.Side);
            string bpTok = BreakpointUtil.GetBreakpointToken(rule.Breakpoint);

            if (!first)
                sb.Append(' ');
            else
                first = false;

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
                BorderRule rule = _rules[i];
                string? sizeVal = GetSizeValue(rule.Size);

                if (sizeVal is null)
                    continue;

                switch (rule.Side)
                {
                    case ElementSide.AllValue:
                        AppendStyle(ref first, ref sb, "border-width", sizeVal);
                        break;

                    case ElementSide.TopValue:
                        AppendStyle(ref first, ref sb, "border-top-width", sizeVal);
                        break;

                    case ElementSide.RightValue:
                        AppendStyle(ref first, ref sb, "border-right-width", sizeVal);
                        break;

                    case ElementSide.BottomValue:
                        AppendStyle(ref first, ref sb, "border-bottom-width", sizeVal);
                        break;

                    case ElementSide.LeftValue:
                        AppendStyle(ref first, ref sb, "border-left-width", sizeVal);
                        break;

                    case ElementSide.HorizontalValue:
                    case ElementSide.LeftRightValue:
                        AppendStyle(ref first, ref sb, "border-left-width", sizeVal);
                        AppendStyle(ref first, ref sb, "border-right-width", sizeVal);
                        break;

                    case ElementSide.VerticalValue:
                    case ElementSide.TopBottomValue:
                        AppendStyle(ref first, ref sb, "border-top-width", sizeVal);
                        AppendStyle(ref first, ref sb, "border-bottom-width", sizeVal);
                        break;

                    case ElementSide.InlineStartValue:
                        AppendStyle(ref first, ref sb, "border-inline-start-width", sizeVal);
                        break;

                    case ElementSide.InlineEndValue:
                        AppendStyle(ref first, ref sb, "border-inline-end-width", sizeVal);
                        break;

                    default:
                        AppendStyle(ref first, ref sb, "border-width", sizeVal);
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
        if (!first) 
            sb.Append("; ");
        else 
            first = false;

        sb.Append(prop);
        sb.Append(": ");
        sb.Append(val);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetSideToken(ElementSide side)
    {
        switch (side)
        {
            case ElementSide.AllValue:
                return string.Empty;
            case ElementSide.TopValue:
                return _sideT;
            case ElementSide.RightValue:
                return _sideE;
            case ElementSide.BottomValue:
                return _sideB;
            case ElementSide.LeftValue:
                return _sideS;
            case ElementSide.HorizontalValue:
            case ElementSide.LeftRightValue:
                return _sideX;
            case ElementSide.VerticalValue:
            case ElementSide.TopBottomValue:
                return _sideY;
            case ElementSide.InlineStartValue:
                return _sideS;
            case ElementSide.InlineEndValue:
                return _sideE;
            default:
                return string.Empty;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string? GetSizeValue(string size)
    {
        return size switch
        {
            Scale.S0Value => "0",
            Scale.S1Value => "1px",
            Scale.S2Value => "2px",
            Scale.S3Value => "3px",
            Scale.S4Value => "4px",
            Scale.S5Value => "5px",
            _ => null
        };
    }

}