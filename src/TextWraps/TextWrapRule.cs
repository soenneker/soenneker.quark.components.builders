using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark;

internal readonly record struct TextWrapRule(string Value, Breakpoint? Breakpoint);
