
namespace Soenneker.Quark;

/// <summary>
/// Represents a single display rule with optional breakpoint.
/// </summary>
internal record DisplayRule(string Display, Breakpoint? Breakpoint = null);
