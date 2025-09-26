
using Soenneker.Quark.Enums;

namespace Soenneker.Quark;

/// <summary>
/// Simplified display utility with fluent API and Bootstrap-first approach.
/// </summary>
public static class Display
{
    /// <summary>
    /// Display none (hidden).
    /// </summary>
    public static DisplayBuilder None => new(Enums.DisplayType.NoneValue);

    /// <summary>
    /// Display inline.
    /// </summary>
    public static DisplayBuilder Inline => new(Enums.DisplayType.InlineValue);

    /// <summary>
    /// Display inline-block.
    /// </summary>
    public static DisplayBuilder InlineBlock => new(Enums.DisplayType.InlineBlockValue);

    /// <summary>
    /// Display block.
    /// </summary>
    public static DisplayBuilder Block => new(Enums.DisplayType.BlockValue);

    /// <summary>
    /// Display flex.
    /// </summary>
    public static DisplayBuilder Flex => new(Enums.DisplayType.FlexValue);

    /// <summary>
    /// Display inline-flex.
    /// </summary>
    public static DisplayBuilder InlineFlex => new(Enums.DisplayType.InlineFlexValue);

    /// <summary>
    /// Display grid.
    /// </summary>
    public static DisplayBuilder Grid => new(Enums.DisplayType.GridValue);

    /// <summary>
    /// Display inline-grid.
    /// </summary>
    public static DisplayBuilder InlineGrid => new(Enums.DisplayType.InlineGridValue);

    /// <summary>
    /// Display table.
    /// </summary>
    public static DisplayBuilder Table => new(Enums.DisplayType.TableValue);

    /// <summary>
    /// Display table-cell.
    /// </summary>
    public static DisplayBuilder TableCell => new(Enums.DisplayType.TableCellValue);

    /// <summary>
    /// Display table-row.
    /// </summary>
    public static DisplayBuilder TableRow => new(Enums.DisplayType.TableRowValue);

    /// <summary>
    /// Inherit from parent.
    /// </summary>
    public static DisplayBuilder Inherit => new(GlobalKeyword.InheritValue);

    /// <summary>
    /// Initial keyword.
    /// </summary>
    public static DisplayBuilder Initial => new(GlobalKeyword.InitialValue);

    /// <summary>
    /// Revert keyword.
    /// </summary>
    public static DisplayBuilder Revert => new(GlobalKeyword.RevertValue);

    /// <summary>
    /// Revert-layer keyword.
    /// </summary>
    public static DisplayBuilder RevertLayer => new(GlobalKeyword.RevertLayerValue);

    /// <summary>
    /// Unset keyword.
    /// </summary>
    public static DisplayBuilder Unset => new(GlobalKeyword.UnsetValue);
}
