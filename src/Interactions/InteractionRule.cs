using Soenneker.Quark.Enums;

namespace Soenneker.Quark;

/// <summary>
/// Represents a single interaction rule with optional breakpoint.
/// </summary>
internal record InteractionRule(string UserSelect, string PointerEvents, Breakpoint? Breakpoint = null);
