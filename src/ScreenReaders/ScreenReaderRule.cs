using Soenneker.Quark.Enums;

namespace Soenneker.Quark;

/// <summary>
/// Represents a single screen reader rule with optional breakpoint.
/// </summary>
internal record ScreenReaderRule(string Type, Breakpoint? Breakpoint = null);
