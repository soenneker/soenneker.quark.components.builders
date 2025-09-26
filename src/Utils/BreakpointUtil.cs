using System.Runtime.CompilerServices;
using Soenneker.Quark.Enums;

namespace Soenneker.Quark;

/// <summary>
/// Shared utilities for converting breakpoints to CSS class tokens.
/// </summary>
public static class BreakpointUtil
{
    /// <summary>
    /// Converts a breakpoint to its corresponding CSS class token.
    /// Returns empty string for phone/extra-small (default) breakpoints.
    /// </summary>
    /// <param name="breakpoint">The breakpoint to convert</param>
    /// <returns>The CSS class token (e.g., "sm", "md", "lg", "xl", "xxl") or empty string</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetBreakpointToken(Breakpoint? breakpoint)
    {
        return breakpoint?.Value ?? string.Empty;
    }

    /// <summary>
    /// Converts a breakpoint to its corresponding CSS class token.
    /// Alias for GetBreakpointToken for backward compatibility.
    /// </summary>
    /// <param name="breakpoint">The breakpoint to convert</param>
    /// <returns>The CSS class token (e.g., "sm", "md", "lg", "xl", "xxl") or empty string</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetBreakpointClass(Breakpoint? breakpoint) => GetBreakpointToken(breakpoint);
}
