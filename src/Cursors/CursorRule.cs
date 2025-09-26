using Soenneker.Quark.Enums;

namespace Soenneker.Quark;

/// <summary>
/// Represents a single cursor rule with optional breakpoint.
/// </summary>
internal record CursorRule(string Cursor, Breakpoint? Breakpoint = null);
