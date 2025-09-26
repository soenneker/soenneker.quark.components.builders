using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark;

/// <summary>
/// Represents a single height rule with optional breakpoint.
/// </summary>
internal record HeightRule(string Size, Breakpoint? Breakpoint = null);
