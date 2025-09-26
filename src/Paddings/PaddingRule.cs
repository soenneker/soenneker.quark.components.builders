
namespace Soenneker.Quark;

/// <summary>
/// Represents a single padding rule with optional breakpoint.
/// </summary>
internal record PaddingRule(string Size, ElementSide Side, Breakpoint? Breakpoint = null);
