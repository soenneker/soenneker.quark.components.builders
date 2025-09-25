using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark;

internal readonly record struct BoxShadowRule(string Value, Breakpoint? Breakpoint);