using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.Builders.TextAlignments;

internal readonly record struct TextAlignmentRule(string Value, Breakpoint? Breakpoint);