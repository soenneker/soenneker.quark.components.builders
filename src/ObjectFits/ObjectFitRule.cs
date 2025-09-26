
namespace Soenneker.Quark;

/// <summary>
/// Represents a single object-fit rule with optional breakpoint.
/// </summary>
internal record ObjectFitRule(string Fit, Breakpoint? Breakpoint = null);
