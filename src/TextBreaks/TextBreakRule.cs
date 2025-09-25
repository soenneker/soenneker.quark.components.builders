using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark;

internal readonly record struct TextBreakRule(bool Enabled, Breakpoint? Breakpoint);