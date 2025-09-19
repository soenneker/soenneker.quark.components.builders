using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Extensions.String;
using Soenneker.Quark.Components.Builders.Abstract;
using Soenneker.Quark.Components.Builders.Utils;
using Soenneker.Quark.Enums.Breakpoints;
using Soenneker.Utils.PooledStringBuilders;

namespace Soenneker.Quark.Components.Builders.Displays;

/// <summary>
/// Simplified display builder with fluent API for chaining display rules.
/// </summary>
public sealed class DisplayBuilder : ICssBuilder
{
    private readonly List<DisplayRule> _rules = new(4);

    internal DisplayBuilder(string display, Breakpoint? breakpoint = null)
    {
        _rules.Add(new DisplayRule(display, breakpoint));
    }

    internal DisplayBuilder(List<DisplayRule> rules)
    {
        if (rules is { Count: > 0 })
            _rules.AddRange(rules);
    }

    public DisplayBuilder None => ChainWithDisplay(Enums.DisplayTypes.DisplayType.NoneValue);
    public DisplayBuilder Inline => ChainWithDisplay(Enums.DisplayTypes.DisplayType.InlineValue);
    public DisplayBuilder InlineBlock => ChainWithDisplay(Enums.DisplayTypes.DisplayType.InlineBlockValue);
    public DisplayBuilder Block => ChainWithDisplay(Enums.DisplayTypes.DisplayType.BlockValue);
    public DisplayBuilder Flex => ChainWithDisplay(Enums.DisplayTypes.DisplayType.FlexValue);
    public DisplayBuilder InlineFlex => ChainWithDisplay(Enums.DisplayTypes.DisplayType.InlineFlexValue);
    public DisplayBuilder Grid => ChainWithDisplay(Enums.DisplayTypes.DisplayType.GridValue);
    public DisplayBuilder InlineGrid => ChainWithDisplay(Enums.DisplayTypes.DisplayType.InlineGridValue);
    public DisplayBuilder Table => ChainWithDisplay(Enums.DisplayTypes.DisplayType.TableValue);
    public DisplayBuilder TableCell => ChainWithDisplay(Enums.DisplayTypes.DisplayType.TableCellValue);
    public DisplayBuilder TableRow => ChainWithDisplay(Enums.DisplayTypes.DisplayType.TableRowValue);
    public DisplayBuilder Inherit => ChainWithDisplay(Enums.GlobalKeywords.GlobalKeyword.InheritValue);
    public DisplayBuilder Initial => ChainWithDisplay(Enums.GlobalKeywords.GlobalKeyword.InitialValue);
    public DisplayBuilder Revert => ChainWithDisplay(Enums.GlobalKeywords.GlobalKeyword.RevertValue);
    public DisplayBuilder RevertLayer => ChainWithDisplay(Enums.GlobalKeywords.GlobalKeyword.RevertLayerValue);
    public DisplayBuilder Unset => ChainWithDisplay(Enums.GlobalKeywords.GlobalKeyword.UnsetValue);

    public DisplayBuilder OnPhone => ChainWithBreakpoint(Breakpoint.Phone);
    public DisplayBuilder OnTablet => ChainWithBreakpoint(Breakpoint.Tablet);
    public DisplayBuilder OnLaptop => ChainWithBreakpoint(Breakpoint.Laptop);
    public DisplayBuilder OnDesktop => ChainWithBreakpoint(Breakpoint.Desktop);
    public DisplayBuilder OnWidescreen => ChainWithBreakpoint(Breakpoint.Widescreen);
    public DisplayBuilder OnUltrawide => ChainWithBreakpoint(Breakpoint.Ultrawide);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private DisplayBuilder ChainWithDisplay(string display)
    {
        _rules.Add(new DisplayRule(display, null));
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private DisplayBuilder ChainWithBreakpoint(Breakpoint breakpoint)
    {
        if (_rules.Count == 0)
        {
            _rules.Add(new DisplayRule(Enums.DisplayTypes.DisplayType.BlockValue, breakpoint));
            return this;
        }

        int lastIdx = _rules.Count - 1;
        DisplayRule last = _rules[lastIdx];
        _rules[lastIdx] = new DisplayRule(last.Display, breakpoint);
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
            DisplayRule rule = _rules[i];
            string cls = GetDisplayClass(rule.Display);
            if (cls.Length == 0)
                continue;

            string bp = BreakpointUtil.GetBreakpointClass(rule.Breakpoint);
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
        if (_rules.Count == 0)
            return string.Empty;

        using var sb = new PooledStringBuilder();
        var first = true;

        for (var i = 0; i < _rules.Count; i++)
        {
            DisplayRule rule = _rules[i];
            string val = rule.Display;
            if (val.IsNullOrEmpty())
                continue;

            if (!first)
                sb.Append("; ");
            else
                first = false;

            sb.Append("display: ");
            sb.Append(val);
        }

        return sb.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetDisplayClass(string display)
    {
        return display switch
        {
            Enums.DisplayTypes.DisplayType.NoneValue => "d-none",
            Enums.DisplayTypes.DisplayType.InlineValue => "d-inline",
            Enums.DisplayTypes.DisplayType.InlineBlockValue => "d-inline-block",
            Enums.DisplayTypes.DisplayType.BlockValue => "d-block",
            Enums.DisplayTypes.DisplayType.FlexValue => "d-flex",
            Enums.DisplayTypes.DisplayType.InlineFlexValue => "d-inline-flex",
            Enums.DisplayTypes.DisplayType.GridValue => "d-grid",
            Enums.DisplayTypes.DisplayType.InlineGridValue => "d-inline-grid",
            Enums.DisplayTypes.DisplayType.TableValue => "d-table",
            Enums.DisplayTypes.DisplayType.TableCellValue => "d-table-cell",
            Enums.DisplayTypes.DisplayType.TableRowValue => "d-table-row",
            _ => string.Empty
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
                s.className.AsSpan(0, s.dashIndex)
                    .CopyTo(dst);
                int idx = s.dashIndex;
                dst[idx++] = '-';
                s.bp.AsSpan()
                    .CopyTo(dst[idx..]);
                idx += s.bp.Length;
                s.className.AsSpan(s.dashIndex)
                    .CopyTo(dst[idx..]);
            });
        }

        return string.Create(bp.Length + 1 + className.Length, (className, bp), static (dst, s) =>
        {
            s.bp.AsSpan()
                .CopyTo(dst);
            int idx = s.bp.Length;
            dst[idx++] = '-';
            s.className.AsSpan()
                .CopyTo(dst[idx..]);
        });
    }
}