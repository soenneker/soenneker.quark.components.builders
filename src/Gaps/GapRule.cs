using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.Builders.Gaps;

/// <summary>
/// Represents a single gap rule with optional breakpoint.
/// </summary>
internal record GapRule(string Size, Breakpoint? Breakpoint = null);