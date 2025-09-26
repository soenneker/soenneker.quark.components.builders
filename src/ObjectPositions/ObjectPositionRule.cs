using Soenneker.Quark.Enums;

namespace Soenneker.Quark;

/// <summary>
/// Represents a single object position rule with optional breakpoint.
/// </summary>
internal record ObjectPositionRule(string Position, Breakpoint? Breakpoint = null);
