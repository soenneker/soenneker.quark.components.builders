using Soenneker.Quark.Enums;

namespace Soenneker.Quark;

/// <summary>
/// Represents a single scroll behavior rule with optional breakpoint.
/// </summary>
internal record ScrollBehaviorRule(string Behavior, Breakpoint? Breakpoint = null);
