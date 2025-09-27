
using Soenneker.Quark.Enums;

namespace Soenneker.Quark;

/// <summary>
/// Represents a single padding rule with optional breakpoint.
/// </summary>
internal record PaddingRule(string Size, ElementSideType Side, Breakpoint? Breakpoint = null);
