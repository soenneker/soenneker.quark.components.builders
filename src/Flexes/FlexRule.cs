using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.Builders.Flexes;

/// <summary>
/// Represents a single flex rule with optional breakpoint.
/// </summary>
internal record FlexRule(string Property, string Value, Breakpoint? Breakpoint = null);
