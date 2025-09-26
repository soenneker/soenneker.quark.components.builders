using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark;

/// <summary>
/// Represents a single gap rule with optional breakpoint.
/// </summary>
internal record GapRule(string Size, Breakpoint? Breakpoint = null);
