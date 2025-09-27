using Soenneker.Quark.Enums;
using TextOverflowEnum = Soenneker.Quark.Enums.TextOverflowKeyword;

namespace Soenneker.Quark;

/// <summary>
/// Represents a single text overflow rule with optional breakpoint.
/// </summary>
internal record TextOverflowRule(string Value, Breakpoint? Breakpoint = null);
