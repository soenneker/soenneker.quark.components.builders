using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.Builders.Positions;

/// <summary>
/// Represents a single position rule with optional breakpoint.
/// </summary>
internal record PositionRule(string Position, Breakpoint? Breakpoint = null);
