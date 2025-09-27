using Soenneker.Quark.Enums;

namespace Soenneker.Quark;

/// <summary>
/// Represents a single text overflow rule with optional breakpoint.
/// </summary>
internal record TextOverflowRule(string Value, Breakpoint? Breakpoint = null);
