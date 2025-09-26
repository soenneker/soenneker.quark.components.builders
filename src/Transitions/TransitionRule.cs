using Soenneker.Quark.Enums;

namespace Soenneker.Quark;

/// <summary>
/// Represents a single transition rule with optional breakpoint.
/// </summary>
internal record TransitionRule(string Transition, Breakpoint? Breakpoint = null);
