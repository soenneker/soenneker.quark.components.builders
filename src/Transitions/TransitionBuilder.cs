using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Quark.Enums;
using Soenneker.Utils.PooledStringBuilders;

namespace Soenneker.Quark;

/// <summary>
/// Simplified transition builder with fluent API for chaining transition rules.
/// </summary>
public sealed class TransitionBuilder : ICssBuilder
{
    private readonly List<TransitionRule> _rules = new(4);

    private const string _classTransitionNone = "transition-none";
    private const string _classTransitionAll = "transition-all";
    private const string _classTransitionColors = "transition-colors";
    private const string _classTransitionOpacity = "transition-opacity";
    private const string _classTransitionShadow = "transition-shadow";
    private const string _classTransitionTransform = "transition-transform";

    internal TransitionBuilder(string transition, Breakpoint? breakpoint = null)
    {
        _rules.Add(new TransitionRule(transition, breakpoint));
    }

    internal TransitionBuilder(List<TransitionRule> rules)
    {
        if (rules is { Count: > 0 })
            _rules.AddRange(rules);
    }

    public TransitionBuilder None => ChainWithTransition("none");
    public TransitionBuilder All => ChainWithTransition("all");
    public TransitionBuilder Colors => ChainWithTransition("colors");
    public TransitionBuilder Opacity => ChainWithTransition("opacity");
    public TransitionBuilder Shadow => ChainWithTransition("shadow");
    public TransitionBuilder Transform => ChainWithTransition("transform");

    public TransitionBuilder OnPhone => ChainWithBreakpoint(Breakpoint.Phone);
    public TransitionBuilder OnTablet => ChainWithBreakpoint(Breakpoint.Tablet);
    public TransitionBuilder OnLaptop => ChainWithBreakpoint(Breakpoint.Laptop);
    public TransitionBuilder OnDesktop => ChainWithBreakpoint(Breakpoint.Desktop);
    public TransitionBuilder OnWidescreen => ChainWithBreakpoint(Breakpoint.Widescreen);
    public TransitionBuilder OnUltrawide => ChainWithBreakpoint(Breakpoint.Ultrawide);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private TransitionBuilder ChainWithTransition(string transition)
    {
        _rules.Add(new TransitionRule(transition, null));
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private TransitionBuilder ChainWithBreakpoint(Breakpoint breakpoint)
    {
        if (_rules.Count == 0)
        {
            _rules.Add(new TransitionRule("none", breakpoint));
            return this;
        }

        int lastIdx = _rules.Count - 1;
        TransitionRule last = _rules[lastIdx];
        _rules[lastIdx] = new TransitionRule(last.Transition, breakpoint);
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
            TransitionRule rule = _rules[i];
            string cls = GetTransitionClass(rule.Transition);
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
            TransitionRule rule = _rules[i];
            string? transitionValue = GetTransitionValue(rule.Transition);

            if (transitionValue is null)
                continue;

            if (!first) sb.Append("; ");
            else first = false;

            sb.Append("transition: ");
            sb.Append(transitionValue);
        }

        return sb.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetTransitionClass(string transition)
    {
        return transition switch
        {
            "none" => _classTransitionNone,
            "all" => _classTransitionAll,
            "colors" => _classTransitionColors,
            "opacity" => _classTransitionOpacity,
            "shadow" => _classTransitionShadow,
            "transform" => _classTransitionTransform,
            _ => string.Empty
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string? GetTransitionValue(string transition)
    {
        return transition switch
        {
            "none" => "none",
            "all" => "all 150ms ease-in-out",
            "colors" => "color 150ms ease-in-out, background-color 150ms ease-in-out, border-color 150ms ease-in-out",
            "opacity" => "opacity 150ms ease-in-out",
            "shadow" => "box-shadow 150ms ease-in-out",
            "transform" => "transform 150ms ease-in-out",
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
