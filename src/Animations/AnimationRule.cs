using Soenneker.Quark.Enums;

namespace Soenneker.Quark;

/// <summary>
/// Represents a single animation rule with optional breakpoint.
/// </summary>
internal record AnimationRule(string Animation, Breakpoint? Breakpoint = null);
