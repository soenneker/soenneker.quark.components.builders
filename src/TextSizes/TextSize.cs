using Soenneker.Quark.Enums;

namespace Soenneker.Quark;

/// <summary>
/// Simplified text size utility with fluent API and Bootstrap-first approach.
/// </summary>
public static class TextSize
{
    /// <summary>
    /// Extra small text size.
    /// </summary>
    public static TextSizeBuilder Xs => new(SizeType.ExtraSmall.Value);

    /// <summary>
    /// Small text size.
    /// </summary>
    public static TextSizeBuilder Sm => new(SizeType.Small.Value);

    /// <summary>
    /// Base text size (default).
    /// </summary>
    public static TextSizeBuilder Base => new("base");

    /// <summary>
    /// Large text size.
    /// </summary>
    public static TextSizeBuilder Lg => new(SizeType.Large.Value);

    /// <summary>
    /// Extra large text size.
    /// </summary>
    public static TextSizeBuilder Xl => new(SizeType.ExtraLarge.Value);

    /// <summary>
    /// 2X large text size.
    /// </summary>
    public static TextSizeBuilder Xl2 => new("2xl");

    /// <summary>
    /// 3X large text size.
    /// </summary>
    public static TextSizeBuilder Xl3 => new("3xl");

    /// <summary>
    /// 4X large text size.
    /// </summary>
    public static TextSizeBuilder Xl4 => new("4xl");

    /// <summary>
    /// 5X large text size.
    /// </summary>
    public static TextSizeBuilder Xl5 => new("5xl");

    /// <summary>
    /// 6X large text size.
    /// </summary>
    public static TextSizeBuilder Xl6 => new("6xl");

    /// <summary>
    /// 7X large text size.
    /// </summary>
    public static TextSizeBuilder Xl7 => new("7xl");

    /// <summary>
    /// 8X large text size.
    /// </summary>
    public static TextSizeBuilder Xl8 => new("8xl");

    /// <summary>
    /// 9X large text size.
    /// </summary>
    public static TextSizeBuilder Xl9 => new("9xl");
}
