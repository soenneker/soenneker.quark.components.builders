using Soenneker.Quark.Enums;

namespace Soenneker.Quark;

/// <summary>
/// Represents a single stretched link rule with optional breakpoint.
/// </summary>
internal record StretchedLinkRule(Breakpoint? Breakpoint = null);
