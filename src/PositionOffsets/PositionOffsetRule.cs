using Soenneker.Quark.Enums.Breakpoints;

namespace Soenneker.Quark.Components.Builders.PositionOffsets;

internal readonly record struct PositionOffsetRule(string Property, string Value, Breakpoint? Breakpoint);