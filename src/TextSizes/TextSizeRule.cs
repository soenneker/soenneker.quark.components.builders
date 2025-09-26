
namespace Soenneker.Quark;

/// <summary>
/// Represents a single text size rule with optional breakpoint.
/// </summary>
internal record TextSizeRule(string Size, Breakpoint? Breakpoint = null);
