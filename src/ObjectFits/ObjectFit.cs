
using Soenneker.Quark.Enums;

namespace Soenneker.Quark;

/// <summary>
/// Simplified object-fit utility with fluent API and Bootstrap-first approach.
/// </summary>
public static class ObjectFit
{
    /// <summary>
    /// object-fit: contain.
    /// </summary>
    public static ObjectFitBuilder Contain => new(Enums.ObjectFitKeyword.ContainValue);

    /// <summary>
    /// object-fit: cover.
    /// </summary>
    public static ObjectFitBuilder Cover => new(Enums.ObjectFitKeyword.CoverValue);

    /// <summary>
    /// object-fit: fill.
    /// </summary>
    public static ObjectFitBuilder Fill => new(Enums.ObjectFitKeyword.FillValue);

    /// <summary>
    /// object-fit: scale-down.
    /// </summary>
    public static ObjectFitBuilder ScaleDown => new(Enums.ObjectFitKeyword.ScaleDownValue);

    /// <summary>
    /// object-fit: none.
    /// </summary>
    public static ObjectFitBuilder None => new(Enums.ObjectFitKeyword.NoneValue);

    public static ObjectFitBuilder Inherit => new(GlobalKeyword.InheritValue);
    public static ObjectFitBuilder Initial => new(GlobalKeyword.InitialValue);
    public static ObjectFitBuilder Revert => new(GlobalKeyword.RevertValue);
    public static ObjectFitBuilder RevertLayer => new(GlobalKeyword.RevertLayerValue);
    public static ObjectFitBuilder Unset => new(GlobalKeyword.UnsetValue);
}
