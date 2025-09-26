using Soenneker.Quark.Enums;

namespace Soenneker.Quark;

/// <summary>
/// Represents a single filter rule with optional breakpoint.
/// </summary>
internal record FilterRule(string Filter, Breakpoint? Breakpoint = null);
