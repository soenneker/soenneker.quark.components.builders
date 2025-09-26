using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Extensions.String;
using Soenneker.Quark.Enums;
using Soenneker.Utils.PooledStringBuilders;

namespace Soenneker.Quark;

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

    public DisplayBuilder None => ChainWithDisplay(Enums.DisplayType.NoneValue);
    public DisplayBuilder Inline => ChainWithDisplay(Enums.DisplayType.InlineValue);
    public DisplayBuilder InlineBlock => ChainWithDisplay(Enums.DisplayType.InlineBlockValue);
    public DisplayBuilder Block => ChainWithDisplay(Enums.DisplayType.BlockValue);
    public DisplayBuilder Flex => ChainWithDisplay(Enums.DisplayType.FlexValue);
    public DisplayBuilder InlineFlex => ChainWithDisplay(Enums.DisplayType.InlineFlexValue);
    public DisplayBuilder Grid => ChainWithDisplay(Enums.DisplayType.GridValue);
    public DisplayBuilder InlineGrid => ChainWithDisplay(Enums.DisplayType.InlineGridValue);
    public DisplayBuilder Table => ChainWithDisplay(Enums.DisplayType.TableValue);
    public DisplayBuilder TableCell => ChainWithDisplay(Enums.DisplayType.TableCellValue);
    public DisplayBuilder TableRow => ChainWithDisplay(Enums.DisplayType.TableRowValue);
    public DisplayBuilder Inherit => ChainWithDisplay(GlobalKeyword.InheritValue);
    public DisplayBuilder Initial => ChainWithDisplay(GlobalKeyword.InitialValue);
    public DisplayBuilder Revert => ChainWithDisplay(GlobalKeyword.RevertValue);
    public DisplayBuilder RevertLayer => ChainWithDisplay(GlobalKeyword.RevertLayerValue);
    public DisplayBuilder Unset => ChainWithDisplay(GlobalKeyword.UnsetValue);

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
            _rules.Add(new DisplayRule(Enums.DisplayType.BlockValue, breakpoint));
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
            DisplayType.NoneValue => "d-none",
            DisplayType.InlineValue => "d-inline",
            DisplayType.InlineBlockValue => "d-inline-block",
            Enums.DisplayType.BlockValue => "d-block",
            Enums.DisplayType.FlexValue => "d-flex",
            Enums.DisplayType.InlineFlexValue => "d-inline-flex",
            Enums.DisplayType.GridValue => "d-grid",
            Enums.DisplayType.InlineGridValue => "d-inline-grid",
            Enums.DisplayType.TableValue => "d-table",
            Enums.DisplayType.TableCellValue => "d-table-cell",
            Enums.DisplayType.TableRowValue => "d-table-row",
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
