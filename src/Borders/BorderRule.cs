
using Soenneker.Quark.Enums;

namespace Soenneker.Quark;

/// <summary>
/// Represents a single border rule with optional breakpoint.
/// </summary>
internal record BorderRule(string Size, ElementSideType Side, Breakpoint? Breakpoint = null);
