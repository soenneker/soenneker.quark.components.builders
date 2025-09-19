using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.Builders.TextSizes;

/// <summary>
/// Represents a single text size rule with optional breakpoint.
/// </summary>
internal record TextSizeRule(string Size, Breakpoint? Breakpoint = null);
