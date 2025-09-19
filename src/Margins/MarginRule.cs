using Soenneker.Quark.Enums.Breakpoints;
using Soenneker.Quark.Enums.ElementSides;

namespace Soenneker.Quark.Components.Builders.Margins;

/// <summary>
/// Represents a single margin rule with optional breakpoint.
/// </summary>
internal record MarginRule(string Size, ElementSide Side, Breakpoint? Breakpoint = null);