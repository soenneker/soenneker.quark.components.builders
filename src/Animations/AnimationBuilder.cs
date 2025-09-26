using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Quark.Enums;
using Soenneker.Utils.PooledStringBuilders;

namespace Soenneker.Quark;

/// <summary>
/// Simplified animation builder with fluent API for chaining animation rules.
/// </summary>
public sealed class AnimationBuilder : ICssBuilder
{
    private readonly List<AnimationRule> _rules = new(4);

    private const string _classAnimationNone = "animate-none";
    private const string _classAnimationSpin = "animate-spin";
    private const string _classAnimationPing = "animate-ping";
    private const string _classAnimationPulse = "animate-pulse";
    private const string _classAnimationBounce = "animate-bounce";

    internal AnimationBuilder(string animation, Breakpoint? breakpoint = null)
    {
        _rules.Add(new AnimationRule(animation, breakpoint));
    }

    internal AnimationBuilder(List<AnimationRule> rules)
    {
        if (rules is { Count: > 0 })
            _rules.AddRange(rules);
    }

    public AnimationBuilder None => ChainWithAnimation("none");
    public AnimationBuilder Spin => ChainWithAnimation("spin");
    public AnimationBuilder Ping => ChainWithAnimation("ping");
    public AnimationBuilder Pulse => ChainWithAnimation("pulse");
    public AnimationBuilder Bounce => ChainWithAnimation("bounce");

    public AnimationBuilder OnPhone => ChainWithBreakpoint(Breakpoint.Phone);
    public AnimationBuilder OnTablet => ChainWithBreakpoint(Breakpoint.Tablet);
    public AnimationBuilder OnLaptop => ChainWithBreakpoint(Breakpoint.Laptop);
    public AnimationBuilder OnDesktop => ChainWithBreakpoint(Breakpoint.Desktop);
    public AnimationBuilder OnWidescreen => ChainWithBreakpoint(Breakpoint.Widescreen);
    public AnimationBuilder OnUltrawide => ChainWithBreakpoint(Breakpoint.Ultrawide);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private AnimationBuilder ChainWithAnimation(string animation)
    {
        _rules.Add(new AnimationRule(animation, null));
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private AnimationBuilder ChainWithBreakpoint(Breakpoint breakpoint)
    {
        if (_rules.Count == 0)
        {
            _rules.Add(new AnimationRule("none", breakpoint));
            return this;
        }

        int lastIdx = _rules.Count - 1;
        AnimationRule last = _rules[lastIdx];
        _rules[lastIdx] = new AnimationRule(last.Animation, breakpoint);
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
            AnimationRule rule = _rules[i];
            string cls = GetAnimationClass(rule.Animation);
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
            AnimationRule rule = _rules[i];
            string? animationValue = GetAnimationValue(rule.Animation);

            if (animationValue is null)
                continue;

            if (!first) sb.Append("; ");
            else first = false;

            sb.Append("animation: ");
            sb.Append(animationValue);
        }

        return sb.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetAnimationClass(string animation)
    {
        return animation switch
        {
            "none" => _classAnimationNone,
            "spin" => _classAnimationSpin,
            "ping" => _classAnimationPing,
            "pulse" => _classAnimationPulse,
            "bounce" => _classAnimationBounce,
            _ => string.Empty
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string? GetAnimationValue(string animation)
    {
        return animation switch
        {
            "none" => "none",
            "spin" => "spin 1s linear infinite",
            "ping" => "ping 1s cubic-bezier(0, 0, 0.2, 1) infinite",
            "pulse" => "pulse 2s cubic-bezier(0.4, 0, 0.6, 1) infinite",
            "bounce" => "bounce 1s infinite",
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
