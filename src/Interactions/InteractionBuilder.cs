using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Quark.Enums;
using Soenneker.Utils.PooledStringBuilders;

namespace Soenneker.Quark;

/// <summary>
/// Simplified interaction builder with fluent API for chaining interaction rules.
/// </summary>
public sealed class InteractionBuilder : ICssBuilder
{
    private readonly List<InteractionRule> _rules = new(4);

    private const string _classUserSelectNone = "user-select-none";
    private const string _classUserSelectAuto = "user-select-auto";
    private const string _classUserSelectText = "user-select-text";
    private const string _classUserSelectAll = "user-select-all";
    private const string _classPointerEventsNone = "pointer-events-none";
    private const string _classPointerEventsAuto = "pointer-events-auto";

    internal InteractionBuilder(string userSelect, string pointerEvents, Breakpoint? breakpoint = null)
    {
        _rules.Add(new InteractionRule(userSelect, pointerEvents, breakpoint));
    }

    internal InteractionBuilder(List<InteractionRule> rules)
    {
        if (rules is { Count: > 0 })
            _rules.AddRange(rules);
    }

    public InteractionBuilder None => ChainWithInteraction("none", "none");
    public InteractionBuilder All => ChainWithInteraction("auto", "auto");
    public InteractionBuilder NoSelect => ChainWithInteraction("none", "auto");
    public InteractionBuilder NoPointer => ChainWithInteraction("auto", "none");
    public InteractionBuilder Text => ChainWithInteraction("text", "auto");
    public InteractionBuilder AllText => ChainWithInteraction("all", "auto");

    public InteractionBuilder OnPhone => ChainWithBreakpoint(Breakpoint.Phone);
    public InteractionBuilder OnTablet => ChainWithBreakpoint(Breakpoint.Tablet);
    public InteractionBuilder OnLaptop => ChainWithBreakpoint(Breakpoint.Laptop);
    public InteractionBuilder OnDesktop => ChainWithBreakpoint(Breakpoint.Desktop);
    public InteractionBuilder OnWidescreen => ChainWithBreakpoint(Breakpoint.Widescreen);
    public InteractionBuilder OnUltrawide => ChainWithBreakpoint(Breakpoint.Ultrawide);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private InteractionBuilder ChainWithInteraction(string userSelect, string pointerEvents)
    {
        _rules.Add(new InteractionRule(userSelect, pointerEvents, null));
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private InteractionBuilder ChainWithBreakpoint(Breakpoint breakpoint)
    {
        if (_rules.Count == 0)
        {
            _rules.Add(new InteractionRule("auto", "auto", breakpoint));
            return this;
        }

        int lastIdx = _rules.Count - 1;
        InteractionRule last = _rules[lastIdx];
        _rules[lastIdx] = new InteractionRule(last.UserSelect, last.PointerEvents, breakpoint);
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
            InteractionRule rule = _rules[i];
            string userSelectClass = GetUserSelectClass(rule.UserSelect);
            string pointerEventsClass = GetPointerEventsClass(rule.PointerEvents);

            if (userSelectClass.Length > 0)
            {
                string bp = BreakpointUtil.GetBreakpointClass(rule.Breakpoint);
                if (bp.Length != 0)
                    userSelectClass = InsertBreakpoint(userSelectClass, bp);

                if (!first) sb.Append(' ');
                else first = false;

                sb.Append(userSelectClass);
            }

            if (pointerEventsClass.Length > 0)
            {
                string bp = BreakpointUtil.GetBreakpointClass(rule.Breakpoint);
                if (bp.Length != 0)
                    pointerEventsClass = InsertBreakpoint(pointerEventsClass, bp);

                if (!first) sb.Append(' ');
                else first = false;

                sb.Append(pointerEventsClass);
            }
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
            InteractionRule rule = _rules[i];
            string? userSelectValue = GetUserSelectValue(rule.UserSelect);
            string? pointerEventsValue = GetPointerEventsValue(rule.PointerEvents);

            if (userSelectValue is not null)
            {
                if (!first) sb.Append("; ");
                else first = false;

                sb.Append("user-select: ");
                sb.Append(userSelectValue);
            }

            if (pointerEventsValue is not null)
            {
                if (!first) sb.Append("; ");
                else first = false;

                sb.Append("pointer-events: ");
                sb.Append(pointerEventsValue);
            }
        }

        return sb.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetUserSelectClass(string userSelect)
    {
        return userSelect switch
        {
            "none" => _classUserSelectNone,
            "auto" => _classUserSelectAuto,
            "text" => _classUserSelectText,
            "all" => _classUserSelectAll,
            _ => string.Empty
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetPointerEventsClass(string pointerEvents)
    {
        return pointerEvents switch
        {
            "none" => _classPointerEventsNone,
            "auto" => _classPointerEventsAuto,
            _ => string.Empty
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string? GetUserSelectValue(string userSelect)
    {
        return userSelect switch
        {
            "none" => "none",
            "auto" => "auto",
            "text" => "text",
            "all" => "all",
            _ => null
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string? GetPointerEventsValue(string pointerEvents)
    {
        return pointerEvents switch
        {
            "none" => "none",
            "auto" => "auto",
            _ => null
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

    public override string ToString()
    {
        return ToClass();
    }
}
