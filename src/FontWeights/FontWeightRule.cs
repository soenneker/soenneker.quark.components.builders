using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark;

internal readonly record struct FontWeightRule(string Value, Breakpoint? Breakpoint);
