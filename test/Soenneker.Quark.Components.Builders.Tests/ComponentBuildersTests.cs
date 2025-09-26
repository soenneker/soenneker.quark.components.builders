using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.Quark.Components.Builders.Tests;

[Collection("Collection")]
public sealed class ComponentBuildersTests : FixturedUnitTest
{
    public ComponentBuildersTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {

    }

    [Fact]
    public void Default()
    {

    }

    [Fact]
    public void ColorBuilder_ThemeColors_GeneratesCorrectOutput()
    {
        // Test Bootstrap theme colors (should generate classes)
        ColorBuilder primary = Color.Primary;
        Assert.Equal("primary", primary.ToClass());
        Assert.Equal("", primary.ToStyle());
        Assert.Equal("primary", primary.ToString());

        ColorBuilder danger = Color.Danger;
        Assert.Equal("danger", danger.ToClass());
        Assert.Equal("", danger.ToStyle());
        Assert.Equal("danger", danger.ToString());
    }

    [Fact]
    public void ColorBuilder_CssKeywords_GeneratesCorrectOutput()
    {
        // Test CSS keywords (should generate styles)
        ColorBuilder inherit = Color.Inherit;
        Assert.Equal("", inherit.ToClass());
        Assert.Equal("inherit", inherit.ToStyle());
        Assert.Equal("inherit", inherit.ToString());

        ColorBuilder initial = Color.Initial;
        Assert.Equal("", initial.ToClass());
        Assert.Equal("initial", initial.ToStyle());
        Assert.Equal("initial", initial.ToString());
    }

    [Fact]
    public void ColorBuilder_ResponsiveColors_WorkCorrectly()
    {
        ColorBuilder responsiveColor = Color.Primary.OnTablet;
        string classResult = responsiveColor.ToClass();
        
        // Should contain both the color class and responsive breakpoint
        // Tablet breakpoint maps to "sm" in Bootstrap
        Assert.Contains("sm-primary", classResult);
        Assert.Contains("sm", classResult);
    }

    [Fact]
    public void ColorBuilder_CustomCssColors_WorkCorrectly()
    {
        // Test custom CSS color values
        ColorBuilder customColor = Color.FromCss("#ff0000");
        Assert.Equal("", customColor.ToClass());
        Assert.Equal("#ff0000", customColor.ToStyle());
        Assert.Equal("#ff0000", customColor.ToString());

        ColorBuilder customColor2 = Color.FromCss("rgb(255, 0, 0)");
        Assert.Equal("", customColor2.ToClass());
        Assert.Equal("rgb(255, 0, 0)", customColor2.ToStyle());
        Assert.Equal("rgb(255, 0, 0)", customColor2.ToString());
    }

    [Fact]
    public void BorderRadiusBuilder_BasicSizes_GeneratesCorrectOutput()
    {
        // Test basic border radius sizes
        BorderRadiusBuilder sm = BorderRadius.Sm;
        Assert.Equal("rounded-sm", sm.ToClass());
        Assert.Equal("border-radius: 0.25rem", sm.ToStyle());
        Assert.Equal("rounded-sm", sm.ToString());

        BorderRadiusBuilder lg = BorderRadius.Lg;
        Assert.Equal("rounded-lg", lg.ToClass());
        Assert.Equal("border-radius: 0.5rem", lg.ToStyle());
        Assert.Equal("rounded-lg", lg.ToString());

        BorderRadiusBuilder pill = BorderRadius.Pill;
        Assert.Equal("rounded-pill", pill.ToClass());
        Assert.Equal("border-radius: 50rem", pill.ToStyle());
        Assert.Equal("rounded-pill", pill.ToString());

        BorderRadiusBuilder circle = BorderRadius.Circle;
        Assert.Equal("rounded-circle", circle.ToClass());
        Assert.Equal("border-radius: 50%", circle.ToStyle());
        Assert.Equal("rounded-circle", circle.ToString());
    }

    [Fact]
    public void BorderRadiusBuilder_CornerSpecific_GeneratesCorrectOutput()
    {
        // Test corner-specific border radius
        BorderRadiusBuilder topLeft = BorderRadius.TopLeft.Lg;
        Assert.Equal("rounded-tl-lg", topLeft.ToClass());
        Assert.Equal("border-top-left-radius: 0.5rem", topLeft.ToStyle());
        Assert.Equal("rounded-tl-lg", topLeft.ToString());

        BorderRadiusBuilder topRight = BorderRadius.TopRight.Sm;
        Assert.Equal("rounded-tr-sm", topRight.ToClass());
        Assert.Equal("border-top-right-radius: 0.25rem", topRight.ToStyle());
        Assert.Equal("rounded-tr-sm", topRight.ToString());

        BorderRadiusBuilder bottomLeft = BorderRadius.BottomLeft.Xl;
        Assert.Equal("rounded-bl-xl", bottomLeft.ToClass());
        Assert.Equal("border-bottom-left-radius: 1rem", bottomLeft.ToStyle());
        Assert.Equal("rounded-bl-xl", bottomLeft.ToString());

        BorderRadiusBuilder bottomRight = BorderRadius.BottomRight.Pill;
        Assert.Equal("rounded-br-pill", bottomRight.ToClass());
        Assert.Equal("border-bottom-right-radius: 50rem", bottomRight.ToStyle());
        Assert.Equal("rounded-br-pill", bottomRight.ToString());
    }

    [Fact]
    public void BorderRadiusBuilder_Responsive_WorksCorrectly()
    {
        BorderRadiusBuilder responsive = BorderRadius.Lg.OnTablet;
        string classResult = responsive.ToClass();
        
        // Should contain both the border radius class and responsive breakpoint
        Assert.Contains("lg", classResult);
        Assert.Contains("sm", classResult); // Tablet breakpoint maps to "sm" in Bootstrap
    }

    [Fact]
    public void BorderRadiusBuilder_Chaining_WorksCorrectly()
    {
        // Test chaining different sizes and corners
        BorderRadiusBuilder chained = BorderRadius.TopLeft.Sm.BottomRight.Lg;
        
        // Debug output
        string classResult = chained.ToClass();
        System.Console.WriteLine($"Class result: '{classResult}'");
        
        // Should contain both corner specifications
        Assert.Contains("tl-sm", classResult);
        Assert.Contains("br-lg", classResult);
        
        string styleResult = chained.ToStyle();
        Assert.Contains("border-top-left-radius: 0.25rem", styleResult);
        Assert.Contains("border-bottom-right-radius: 0.5rem", styleResult);
    }
}
