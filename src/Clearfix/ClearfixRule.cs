using Soenneker.Quark.Enums;

namespace Soenneker.Quark;

/// <summary>
/// Represents a single clearfix rule with optional breakpoint.
/// </summary>
internal record ClearfixRule(Breakpoint? Breakpoint = null);
