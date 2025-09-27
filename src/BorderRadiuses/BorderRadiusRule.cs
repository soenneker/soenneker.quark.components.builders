using Soenneker.Quark.Enums;

namespace Soenneker.Quark;

/// <summary>
/// Represents a single border radius rule with optional breakpoint.
/// </summary>
internal record BorderRadiusRule(string Size, ElementSideType Side, Breakpoint? Breakpoint = null, string CornerToken = "");
