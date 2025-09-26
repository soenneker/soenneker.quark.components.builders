
namespace Soenneker.Quark;

/// <summary>
/// Represents a single border rule with optional breakpoint.
/// </summary>
internal record BorderRule(string Size, ElementSide Side, Breakpoint? Breakpoint = null);
