using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark;

internal readonly record struct ColorRule(string Value, Breakpoint? Breakpoint);