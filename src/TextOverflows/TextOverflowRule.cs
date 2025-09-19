using Soenneker.Quark.Enums.Breakpoints;
using TextOverflowEnum = Soenneker.Quark.Enums.TextOverflows.TextOverflow;

namespace Soenneker.Quark.Components.Builders.TextOverflows;

/// <summary>
/// Represents a single text overflow rule with optional breakpoint.
/// </summary>
internal record TextOverflowRule(TextOverflowEnum TextOverflow, Breakpoint? Breakpoint = null);
