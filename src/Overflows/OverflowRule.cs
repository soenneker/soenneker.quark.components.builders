using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark;

/// <summary>
/// Represents a single overflow rule with optional breakpoint.
/// </summary>
internal record OverflowRule(string Overflow, Breakpoint? Breakpoint = null);
