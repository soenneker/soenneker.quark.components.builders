
using Soenneker.Quark.Enums;

namespace Soenneker.Quark;

/// <summary>
/// Represents a single flex rule with optional breakpoint.
/// </summary>
internal record FlexRule(string Property, string Value, Breakpoint? Breakpoint = null);
