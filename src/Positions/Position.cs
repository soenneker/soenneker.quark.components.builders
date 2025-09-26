
using Soenneker.Quark.Enums;

namespace Soenneker.Quark;

/// <summary>
/// Simplified position utility with fluent API and Bootstrap-first approach.
/// </summary>
public static class Position
{
    /// <summary>
    /// Static positioning.
    /// </summary>
    public static PositionBuilder Static => new(Enums.Position.StaticValue);

    /// <summary>
    /// Relative positioning.
    /// </summary>
    public static PositionBuilder Relative => new(Enums.Position.RelativeValue);

    /// <summary>
    /// Absolute positioning.
    /// </summary>
    public static PositionBuilder Absolute => new(Enums.Position.AbsoluteValue);

    /// <summary>
    /// Fixed positioning.
    /// </summary>
    public static PositionBuilder Fixed => new(Enums.Position.FixedValue);

    /// <summary>
    /// Sticky positioning.
    /// </summary>
    public static PositionBuilder Sticky => new(Enums.Position.StickyValue);

    public static PositionBuilder Inherit => new(GlobalKeyword.InheritValue);
    public static PositionBuilder Initial => new(GlobalKeyword.InitialValue);
    public static PositionBuilder Revert => new(GlobalKeyword.RevertValue);
    public static PositionBuilder RevertLayer => new(GlobalKeyword.RevertLayerValue);
    public static PositionBuilder Unset => new(GlobalKeyword.UnsetValue);
}
