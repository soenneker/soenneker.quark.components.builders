using Soenneker.Quark.Enums.Breakpoints;
using Soenneker.Quark.Enums.ElementSides;

namespace Soenneker.Quark.Components.Builders.Paddings;

/// <summary>
/// Represents a single padding rule with optional breakpoint.
/// </summary>
internal record PaddingRule(string Size, ElementSide Side, Breakpoint? Breakpoint = null);
