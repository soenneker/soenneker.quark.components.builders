
namespace Soenneker.Quark;

/// <summary>
/// Represents a single width rule with optional breakpoint.
/// </summary>
internal record WidthRule(string Size, Breakpoint? Breakpoint = null);


