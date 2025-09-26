
using Soenneker.Quark.Enums;

namespace Soenneker.Quark;

/// <summary>
/// Represents a single position rule with optional breakpoint.
/// </summary>
internal record PositionRule(string Position, Breakpoint? Breakpoint = null);
