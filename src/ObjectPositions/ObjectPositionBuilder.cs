using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Quark.Enums;
using Soenneker.Utils.PooledStringBuilders;

namespace Soenneker.Quark;

/// <summary>
/// Simplified object position builder with fluent API for chaining object position rules.
/// </summary>
public sealed class ObjectPositionBuilder : ICssBuilder
{
    private readonly List<ObjectPositionRule> _rules = new(4);

    private const string _classObjectPositionCenter = "object-center";
    private const string _classObjectPositionTop = "object-top";
    private const string _classObjectPositionRight = "object-right";
    private const string _classObjectPositionBottom = "object-bottom";
    private const string _classObjectPositionLeft = "object-left";
    private const string _classObjectPositionTopLeft = "object-top-left";
    private const string _classObjectPositionTopRight = "object-top-right";
    private const string _classObjectPositionBottomLeft = "object-bottom-left";
    private const string _classObjectPositionBottomRight = "object-bottom-right";

    internal ObjectPositionBuilder(string position, Breakpoint? breakpoint = null)
    {
        _rules.Add(new ObjectPositionRule(position, breakpoint));
    }

    internal ObjectPositionBuilder(List<ObjectPositionRule> rules)
    {
        if (rules is { Count: > 0 })
            _rules.AddRange(rules);
    }

    public ObjectPositionBuilder Center => ChainWithPosition("center");
    public ObjectPositionBuilder Top => ChainWithPosition("top");
    public ObjectPositionBuilder Right => ChainWithPosition("right");
    public ObjectPositionBuilder Bottom => ChainWithPosition("bottom");
    public ObjectPositionBuilder Left => ChainWithPosition("left");
    public ObjectPositionBuilder TopLeft => ChainWithPosition("top-left");
    public ObjectPositionBuilder TopRight => ChainWithPosition("top-right");
    public ObjectPositionBuilder BottomLeft => ChainWithPosition("bottom-left");
    public ObjectPositionBuilder BottomRight => ChainWithPosition("bottom-right");

    public ObjectPositionBuilder OnPhone => ChainWithBreakpoint(Breakpoint.Phone);
    public ObjectPositionBuilder OnTablet => ChainWithBreakpoint(Breakpoint.Tablet);
    public ObjectPositionBuilder OnLaptop => ChainWithBreakpoint(Breakpoint.Laptop);
    public ObjectPositionBuilder OnDesktop => ChainWithBreakpoint(Breakpoint.Desktop);
    public ObjectPositionBuilder OnWidescreen => ChainWithBreakpoint(Breakpoint.Widescreen);
    public ObjectPositionBuilder OnUltrawide => ChainWithBreakpoint(Breakpoint.Ultrawide);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ObjectPositionBuilder ChainWithPosition(string position)
    {
        _rules.Add(new ObjectPositionRule(position, null));
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ObjectPositionBuilder ChainWithBreakpoint(Breakpoint breakpoint)
    {
        if (_rules.Count == 0)
        {
            _rules.Add(new ObjectPositionRule("center", breakpoint));
            return this;
        }

        int lastIdx = _rules.Count - 1;
        ObjectPositionRule last = _rules[lastIdx];
        _rules[lastIdx] = new ObjectPositionRule(last.Position, breakpoint);
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
            ObjectPositionRule rule = _rules[i];
            string cls = GetObjectPositionClass(rule.Position);
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
            ObjectPositionRule rule = _rules[i];
            string? positionValue = GetObjectPositionValue(rule.Position);

            if (positionValue is null)
                continue;

            if (!first) sb.Append("; ");
            else first = false;

            sb.Append("object-position: ");
            sb.Append(positionValue);
        }

        return sb.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string GetObjectPositionClass(string position)
    {
        return position switch
        {
            "center" => _classObjectPositionCenter,
            "top" => _classObjectPositionTop,
            "right" => _classObjectPositionRight,
            "bottom" => _classObjectPositionBottom,
            "left" => _classObjectPositionLeft,
            "top-left" => _classObjectPositionTopLeft,
            "top-right" => _classObjectPositionTopRight,
            "bottom-left" => _classObjectPositionBottomLeft,
            "bottom-right" => _classObjectPositionBottomRight,
            _ => string.Empty
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string? GetObjectPositionValue(string position)
    {
        return position switch
        {
            "center" => "center",
            "top" => "top",
            "right" => "right",
            "bottom" => "bottom",
            "left" => "left",
            "top-left" => "top left",
            "top-right" => "top right",
            "bottom-left" => "bottom left",
            "bottom-right" => "bottom right",
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
