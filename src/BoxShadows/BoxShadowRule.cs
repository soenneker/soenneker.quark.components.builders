using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.Builders.BoxShadows;

internal readonly record struct BoxShadowRule(string Value, Breakpoint? Breakpoint);