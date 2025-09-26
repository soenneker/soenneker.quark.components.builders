using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Utils.PooledStringBuilders;

namespace Soenneker.Quark;

/// <summary>
/// Simplified margin builder with fluent API for chaining margin rules.
/// </summary>
public sealed class MarginBuilder : ICssBuilder
{
    private readonly List<MarginRule> _rules = new(4);

    // ----- Class tokens -----
    private const string _baseToken = "m";

    // ----- Size tokens -----
    private const string _token0 = "0";
    private const string _token1 = "1";
    private const string _token2 = "2";
    private const string _token3 = "3";
    private const string _token4 = "4";
    private const string _token5 = "5";
    private const string _tokenAuto = "auto";

    // ----- Side tokens (Bootstrap naming) -----
    private const string _sideT = "t";
    private const string _sideE = "e";
    private const string _sideB = "b";
    private const string _sideS = "s";
    private const string _sideX = "x";
    private const string _sideY = "y";

    internal MarginBuilder(string size, Breakpoint? breakpoint = null)
    {
        _rules.Add(new MarginRule(size, ElementSide.All, breakpoint));
    }

    internal MarginBuilder(List<MarginRule> rules)
    {
        if (rules is { Count: > 0 })
            _rules.AddRange(rules);
    }

    // ----- Side chaining -----
    public MarginBuilder FromTop => AddRule(ElementSide.Top);
    public MarginBuilder FromRight => AddRule(ElementSide.Right);
    public MarginBuilder FromBottom => AddRule(ElementSide.Bottom);
    public MarginBuilder FromLeft => AddRule(ElementSide.Left);
    public MarginBuilder OnX => AddRule(ElementSide.Horizontal);
    public MarginBuilder OnY => AddRule(ElementSide.Vertical);
    public MarginBuilder OnAll => AddRule(ElementSide.All);
    public MarginBuilder FromStart => AddRule(ElementSide.InlineStart);
    public MarginBuilder FromEnd => AddRule(ElementSide.InlineEnd);

    // ----- Size chaining -----
    public MarginBuilder S0 => ChainWithSize(Scale.S0);
    public MarginBuilder S1 => ChainWithSize(Scale.S1);
    public MarginBuilder S2 => ChainWithSize(Scale.S2);
    public MarginBuilder S3 => ChainWithSize(Scale.S3);
    public MarginBuilder S4 => ChainWithSize(Scale.S4);
    public MarginBuilder S5 => ChainWithSize(Scale.S5);
    public MarginBuilder Auto => ChainWithSize("auto");

    // ----- Breakpoint chaining -----
    public MarginBuilder OnPhone => ChainWithBreakpoint(Breakpoint.Phone);
    public MarginBuilder OnTablet => ChainWithBreakpoint(Breakpoint.Tablet);
    public MarginBuilder OnLaptop => ChainWithBreakpoint(Breakpoint.Laptop);
    public MarginBuilder OnDesktop => ChainWithBreakpoint(Breakpoint.Desktop);
    public MarginBuilder OnWidescreen => ChainWithBreakpoint(Breakpoint.Widescreen);
    public MarginBuilder OnUltrawide => ChainWithBreakpoint(Breakpoint.Ultrawide);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private MarginBuilder AddRule(ElementSide side)
    {
        string size = _rules.Count > 0 ? _rules[^1].Size : Scale.S0Value;
        Breakpoint? bp = _rules.Count > 0 ? _rules[^1].Breakpoint : null;

        if (_rules.Count > 0 && _rules[^1].Side == ElementSide.All)
        {
            _rules[^1] = new MarginRule(size, side, bp);
        }
        else
        {
            _rules.Add(new MarginRule(size, side, bp));
        }

        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private MarginBuilder ChainWithSize(string size)
    {
        _rules.Add(new MarginRule(size, ElementSide.All, null));
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private MarginBuilder ChainWithSize(Scale scale)
    {
        _rules.Add(new MarginRule(scale.Value, ElementSide.All, null));
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private MarginBuilder ChainWithBreakpoint(Breakpoint breakpoint)
    {
        if (_rules.Count == 0)
        {
            _rules.Add(new MarginRule(Scale.S0Value, ElementSide.All, breakpoint));
            return this;
        }

        int lastIdx = _rules.Count - 1;
        MarginRule last = _rules[lastIdx];
        _rules[lastIdx] = new MarginRule(last.Size, last.Side, breakpoint);
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
            MarginRule rule = _rules[i];

            string sizeTok = GetSizeToken(rule.Size);

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
                MarginRule rule = _rules[i];
                string? sizeVal = GetSizeValue(rule.Size);
                if (sizeVal is null)
                    continue;

                switch (rule.Side)
                {
                    case ElementSide.AllValue:
                        AppendStyle(ref first, ref sb, "margin", sizeVal);
                        break;

                    case ElementSide.TopValue:
                        AppendStyle(ref first, ref sb, "margin-top", sizeVal);
                        break;

                    case ElementSide.RightValue:
                        AppendStyle(ref first, ref sb, "margin-right", sizeVal);
                        break;

                    case ElementSide.BottomValue:
                        AppendStyle(ref first, ref sb, "margin-bottom", sizeVal);
                        break;

                    case ElementSide.LeftValue:
                        AppendStyle(ref first, ref sb, "margin-left", sizeVal);
                        break;

                    case ElementSide.HorizontalValue:
                    case ElementSide.LeftRightValue:
                        AppendStyle(ref first, ref sb, "margin-left", sizeVal);
                        AppendStyle(ref first, ref sb, "margin-right", sizeVal);
                        break;

                    case ElementSide.VerticalValue:
                    case ElementSide.TopBottomValue:
                        AppendStyle(ref first, ref sb, "margin-top", sizeVal);
                        AppendStyle(ref first, ref sb, "margin-bottom", sizeVal);
                        break;

                    case ElementSide.InlineStartValue:
                        AppendStyle(ref first, ref sb, "margin-inline-start", sizeVal);
                        break;

                    case ElementSide.InlineEndValue:
                        AppendStyle(ref first, ref sb, "margin-inline-end", sizeVal);
                        break;

                    default:
                        AppendStyle(ref first, ref sb, "margin", sizeVal);
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
                Scale.S0Value => _token0,
                Scale.S1Value => _token1,
                Scale.S2Value => _token2,
                Scale.S3Value => _token3,
                Scale.S4Value => _token4,
                Scale.S5Value => _token5,
                "auto" => _tokenAuto,
                _ => string.Empty
            };
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
                Scale.S1Value => "0.25rem",
                Scale.S2Value => "0.5rem",
                Scale.S3Value => "1rem",
                Scale.S4Value => "1.5rem",
                Scale.S5Value => "3rem",
                "auto" => "auto",
                _ => null
            };
        }

    }