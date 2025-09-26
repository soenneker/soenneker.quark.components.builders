using Soenneker.Quark.Enums;

namespace Soenneker.Quark;

/// <summary>
/// Represents a single backdrop filter rule with optional breakpoint.
/// </summary>
internal record BackdropFilterRule(string Filter, Breakpoint? Breakpoint = null);
