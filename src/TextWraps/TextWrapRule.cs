using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.Builders.TextWraps;

internal readonly record struct TextWrapRule(string Value, Breakpoint? Breakpoint);