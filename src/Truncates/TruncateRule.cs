using Soenneker.Quark.Enums;

namespace Soenneker.Quark;

/// <summary>
/// Represents a single truncate rule with optional breakpoint.
/// </summary>
internal record TruncateRule(Breakpoint? Breakpoint = null);
