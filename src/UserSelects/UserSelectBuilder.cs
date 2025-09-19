using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soenneker.Quark.Components.Builders.Abstract;
using Soenneker.Quark.Components.Builders.Utils;
using Soenneker.Quark.Enums.Breakpoints;
using Soenneker.Utils.PooledStringBuilders;

namespace Soenneker.Quark.Components.Builders.UserSelects;

public sealed class UserSelectBuilder : ICssBuilder
{
    private readonly List<UserSelectRule> _rules = new(4);

    private const string _classNone = "user-select-none";
    private const string _classAuto = "user-select-auto";
    private const string _classAll = "user-select-all";
    private const string _stylePrefix = "user-select: ";

    internal UserSelectBuilder(string value, Breakpoint? breakpoint = null)
    {
        _rules.Add(new UserSelectRule(value, breakpoint));
    }

    internal UserSelectBuilder(List<UserSelectRule> rules)
    {
        if (rules is { Count: > 0 })
            _rules.AddRange(rules);
    }

    public UserSelectBuilder None => Chain(Enums.UserSelects.UserSelect.NoneValue);
    public UserSelectBuilder Auto => Chain(Enums.UserSelects.UserSelect.AutoValue);
    public UserSelectBuilder All => Chain(Enums.UserSelects.UserSelect.AllValue);
    public UserSelectBuilder Inherit => Chain(Enums.GlobalKeywords.GlobalKeyword.InheritValue);
    public UserSelectBuilder Initial => Chain(Enums.GlobalKeywords.GlobalKeyword.InitialValue);
    public UserSelectBuilder Revert => Chain(Enums.GlobalKeywords.GlobalKeyword.RevertValue);
    public UserSelectBuilder RevertLayer => Chain(Enums.GlobalKeywords.GlobalKeyword.RevertLayerValue);
    public UserSelectBuilder Unset => Chain(Enums.GlobalKeywords.GlobalKeyword.UnsetValue);

    public UserSelectBuilder OnPhone => ChainBp(Breakpoint.Phone);
    public UserSelectBuilder OnTablet => ChainBp(Breakpoint.Tablet);
    public UserSelectBuilder OnLaptop => ChainBp(Breakpoint.Laptop);
    public UserSelectBuilder OnDesktop => ChainBp(Breakpoint.Desktop);
    public UserSelectBuilder OnWidescreen => ChainBp(Breakpoint.Widescreen);
    public UserSelectBuilder OnUltrawide => ChainBp(Breakpoint.Ultrawide);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private UserSelectBuilder Chain(string value)
    {
        _rules.Add(new UserSelectRule(value, null));
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private UserSelectBuilder ChainBp(Breakpoint bp)
    {
        if (_rules.Count == 0)
        {
            _rules.Add(new UserSelectRule(Enums.UserSelects.UserSelect.AutoValue, bp));
            return this;
        }

        int lastIdx = _rules.Count - 1;
        UserSelectRule last = _rules[lastIdx];
        _rules[lastIdx] = new UserSelectRule(last.Value, bp);
        return this;
    }

    public string ToClass()
    {
        if (_rules.Count == 0) return string.Empty;

        using var sb = new PooledStringBuilder();
        var first = true;
        for (var i = 0; i < _rules.Count; i++)
        {
            UserSelectRule rule = _rules[i];
            string cls = rule.Value switch
            {
                Enums.UserSelects.UserSelect.NoneValue => _classNone,
                Enums.UserSelects.UserSelect.AutoValue => _classAuto,
                Enums.UserSelects.UserSelect.AllValue => _classAll,
                _ => string.Empty
            };
            if (cls.Length == 0)
                continue;

            string bp = BreakpointUtil.GetBreakpointToken(rule.Breakpoint);
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
        if (_rules.Count == 0) return string.Empty;

        using var sb = new PooledStringBuilder();
        var first = true;
        for (var i = 0; i < _rules.Count; i++)
        {
            UserSelectRule rule = _rules[i];
            string val = rule.Value;
            if (string.IsNullOrEmpty(val))
                continue;

            if (!first) sb.Append("; ");
            else first = false;

            sb.Append(_stylePrefix);
            sb.Append(val);
        }
        return sb.ToString();
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
}

 
