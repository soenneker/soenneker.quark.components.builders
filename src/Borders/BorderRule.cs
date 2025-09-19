using Soenneker.Quark.Enums.Breakpoints;
using Soenneker.Quark.Enums.ElementSides;

namespace Soenneker.Quark.Components.Builders.Borders;

/// <summary>
/// Represents a single border rule with optional breakpoint.
/// </summary>
internal record BorderRule(string Size, ElementSide Side, Breakpoint? Breakpoint = null);
