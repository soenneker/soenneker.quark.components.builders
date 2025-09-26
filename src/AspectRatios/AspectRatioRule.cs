using Soenneker.Quark.Enums;

namespace Soenneker.Quark;

/// <summary>
/// Represents a single aspect ratio rule with optional breakpoint.
/// </summary>
internal record AspectRatioRule(string Ratio, Breakpoint? Breakpoint = null);
