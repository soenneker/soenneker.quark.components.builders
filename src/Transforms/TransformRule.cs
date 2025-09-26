using Soenneker.Quark.Enums;

namespace Soenneker.Quark;

/// <summary>
/// Represents a single transform rule with optional breakpoint.
/// </summary>
internal record TransformRule(string Transform, Breakpoint? Breakpoint = null);
