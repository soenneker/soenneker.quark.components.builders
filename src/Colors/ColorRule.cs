using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.Builders.Colors;

internal readonly record struct ColorRule(string Value, Breakpoint? Breakpoint);