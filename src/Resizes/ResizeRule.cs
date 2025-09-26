using Soenneker.Quark.Enums;

namespace Soenneker.Quark;

/// <summary>
/// Represents a single resize rule with optional breakpoint.
/// </summary>
internal record ResizeRule(string Resize, Breakpoint? Breakpoint = null);
