using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.Builders.TextBreaks;

internal readonly record struct TextBreakRule(bool Enabled, Breakpoint? Breakpoint);