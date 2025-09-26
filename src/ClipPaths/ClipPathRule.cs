using Soenneker.Quark.Enums;

namespace Soenneker.Quark;

/// <summary>
/// Represents a single clip path rule with optional breakpoint.
/// </summary>
internal record ClipPathRule(string Path, Breakpoint? Breakpoint = null);
