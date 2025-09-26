using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Quark.Enums;
using Soenneker.Utils.PooledStringBuilders;

namespace Soenneker.Quark;

public sealed class PointerEventsBuilder : ICssBuilder
{
    private readonly List<PointerEventsRule> _rules = new(4);

    // ----- Class constants -----
    private const string _classNone = "pe-none";
    private const string _classAuto = "pe-auto";

    // ----- CSS prefix & style constants (Intellenum *Value are const, so const-interp is fine) -----
    private const string _pointerEventsPrefix = "pointer-events: ";

    private const string _styleNone = $"{_pointerEventsPrefix}none";
    private const string _styleAuto = $"{_pointerEventsPrefix}auto";
    private const string _styleInherit = $"{_pointerEventsPrefix}{GlobalKeyword.InheritValue}";
    private const string _styleInitial = $"{_pointerEventsPrefix}{GlobalKeyword.InitialValue}";
    private const string _styleUnset = $"{_pointerEventsPrefix}{GlobalKeyword.UnsetValue}";
    private const string _styleRevert = $"{_pointerEventsPrefix}{GlobalKeyword.RevertValue}";
    private const string _styleRevertLayer = $"{_pointerEventsPrefix}{GlobalKeyword.RevertLayerValue}";

    internal PointerEventsBuilder(string value, Breakpoint? breakpoint = null)
    {
        _rules.Add(new PointerEventsRule(value, breakpoint));
    }

    internal PointerEventsBuilder(List<PointerEventsRule> rules)
    {
        if (rules is { Count: > 0 })
            _rules.AddRange(rules);
    }

    public PointerEventsBuilder None => Chain(Enums.PointerEvents.None);
    public PointerEventsBuilder Auto => Chain(Enums.PointerEvents.Auto);
    public PointerEventsBuilder Inherit => Chain(GlobalKeyword.InheritValue);
    public PointerEventsBuilder Initial => Chain(GlobalKeyword.InitialValue);
    public PointerEventsBuilder Revert => Chain(GlobalKeyword.RevertValue);
    public PointerEventsBuilder RevertLayer => Chain(GlobalKeyword.RevertLayerValue);
    public PointerEventsBuilder Unset => Chain(GlobalKeyword.UnsetValue);

    public PointerEventsBuilder OnPhone => ChainBp(Breakpoint.Phone);
    public PointerEventsBuilder OnTablet => ChainBp(Breakpoint.Tablet);
    public PointerEventsBuilder OnLaptop => ChainBp(Breakpoint.Laptop);
    public PointerEventsBuilder OnDesktop => ChainBp(Breakpoint.Desktop);
    public PointerEventsBuilder OnWidescreen => ChainBp(Breakpoint.Widescreen);
    public PointerEventsBuilder OnUltrawide => ChainBp(Breakpoint.Ultrawide);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private PointerEventsBuilder Chain(string value)
    {
        _rules.Add(new PointerEventsRule(value, null));
        return this;
    }

    /// <summary>Apply a breakpoint to the most recent rule (or bootstrap with "auto" if empty).</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private PointerEventsBuilder ChainBp(Breakpoint bp)
    {
        if (_rules.Count == 0)
        {
            _rules.Add(new PointerEventsRule(Enums.PointerEvents.Auto, bp));
            return this;
        }

        int lastIdx = _rules.Count - 1;
        PointerEventsRule last = _rules[lastIdx];
        _rules[lastIdx] = new PointerEventsRule(last.Value, bp);
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
            PointerEventsRule rule = _rules[i];

            string baseClass = rule.Value switch
            {
                Enums.PointerEvents.NoneValue => _classNone,
                Enums.PointerEvents.AutoValue => _classAuto,
                _ => string.Empty
            };

            if (baseClass.Length == 0)
                continue;

            string bp = BreakpointUtil.GetBreakpointToken(rule.Breakpoint);
            if (bp.Length != 0)
                baseClass = InsertBreakpoint(baseClass, bp);

            if (!first) sb.Append(' ');
            else first = false;

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
            PointerEventsRule rule = _rules[i];

            string css = rule.Value switch
            {
                Enums.PointerEvents.NoneValue => _styleNone,
                Enums.PointerEvents.AutoValue => _styleAuto,
                GlobalKeyword.InheritValue => _styleInherit,
                GlobalKeyword.InitialValue => _styleInitial,
                GlobalKeyword.UnsetValue => _styleUnset,
                GlobalKeyword.RevertValue => _styleRevert,
                GlobalKeyword.RevertLayerValue => _styleRevertLayer,
                _ => string.Empty
            };

            if (css.Length == 0)
                continue;

            if (!first) sb.Append("; ");
            else first = false;

            sb.Append(css);
        }

        return sb.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetBp(Breakpoint? breakpoint) => breakpoint?.Value ?? string.Empty;

    /// <summary>
    /// Insert breakpoint token as: "pe-auto" + "md" ? "pe-md-auto".
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
