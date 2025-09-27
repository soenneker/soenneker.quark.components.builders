using Soenneker.Tests.FixturedUnit;
using Xunit;
using AwesomeAssertions;

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
        primary.ToClass().Should().Be("primary");
        primary.ToStyle().Should().Be("");
        primary.ToString().Should().Be("primary");

        ColorBuilder danger = Color.Danger;
        danger.ToClass().Should().Be("danger");
        danger.ToStyle().Should().Be("");
        danger.ToString().Should().Be("danger");
    }

    [Fact]
    public void ColorBuilder_CssKeywords_GeneratesCorrectOutput()
    {
        // Test CSS keywords (should generate styles)
        ColorBuilder inherit = Color.Inherit;
        inherit.ToClass().Should().Be("");
        inherit.ToStyle().Should().Be("inherit");
        inherit.ToString().Should().Be("inherit");

        ColorBuilder initial = Color.Initial;
        initial.ToClass().Should().Be("");
        initial.ToStyle().Should().Be("initial");
        initial.ToString().Should().Be("initial");
    }

    [Fact]
    public void ColorBuilder_ResponsiveColors_WorkCorrectly()
    {
        ColorBuilder responsiveColor = Color.Primary.OnTablet;
        string classResult = responsiveColor.ToClass();
        
        // Should contain both the color class and responsive breakpoint
        // Tablet breakpoint maps to "sm" in Bootstrap
        classResult.Should().Contain("sm-primary");
        classResult.Should().Contain("sm");
    }

    [Fact]
    public void ColorBuilder_CustomCssColors_WorkCorrectly()
    {
        // Test custom CSS color values
        ColorBuilder customColor = Color.FromCss("#ff0000");
        customColor.ToClass().Should().Be("");
        customColor.ToStyle().Should().Be("#ff0000");
        customColor.ToString().Should().Be("#ff0000");

        ColorBuilder customColor2 = Color.FromCss("rgb(255, 0, 0)");
        customColor2.ToClass().Should().Be("");
        customColor2.ToStyle().Should().Be("rgb(255, 0, 0)");
        customColor2.ToString().Should().Be("rgb(255, 0, 0)");
    }

    [Fact]
    public void BorderRadiusBuilder_BasicSizes_GeneratesCorrectOutput()
    {
        // Test basic border radius sizes
        BorderRadiusBuilder sm = BorderRadius.Sm;
        sm.ToClass().Should().Be("rounded-sm");
        sm.ToStyle().Should().Be("border-radius: 0.25rem");
        sm.ToString().Should().Be("rounded-sm");

        BorderRadiusBuilder lg = BorderRadius.Lg;
        lg.ToClass().Should().Be("rounded-lg");
        lg.ToStyle().Should().Be("border-radius: 0.5rem");
        lg.ToString().Should().Be("rounded-lg");

        BorderRadiusBuilder pill = BorderRadius.Pill;
        pill.ToClass().Should().Be("rounded-pill");
        pill.ToStyle().Should().Be("border-radius: 50rem");
        pill.ToString().Should().Be("rounded-pill");

        BorderRadiusBuilder circle = BorderRadius.Circle;
        circle.ToClass().Should().Be("rounded-circle");
        circle.ToStyle().Should().Be("border-radius: 50%");
        circle.ToString().Should().Be("rounded-circle");
    }

    [Fact]
    public void BorderRadiusBuilder_CornerSpecific_GeneratesCorrectOutput()
    {
        // Test corner-specific border radius
        BorderRadiusBuilder topLeft = BorderRadius.TopLeft.Lg;
        topLeft.ToClass().Should().Be("rounded-tl-lg");
        topLeft.ToStyle().Should().Be("border-top-left-radius: 0.5rem");
        topLeft.ToString().Should().Be("rounded-tl-lg");

        BorderRadiusBuilder topRight = BorderRadius.TopRight.Sm;
        topRight.ToClass().Should().Be("rounded-tr-sm");
        topRight.ToStyle().Should().Be("border-top-right-radius: 0.25rem");
        topRight.ToString().Should().Be("rounded-tr-sm");

        BorderRadiusBuilder bottomLeft = BorderRadius.BottomLeft.Xl;
        bottomLeft.ToClass().Should().Be("rounded-bl-xl");
        bottomLeft.ToStyle().Should().Be("border-bottom-left-radius: 1rem");
        bottomLeft.ToString().Should().Be("rounded-bl-xl");

        BorderRadiusBuilder bottomRight = BorderRadius.BottomRight.Pill;
        bottomRight.ToClass().Should().Be("rounded-br-pill");
        bottomRight.ToStyle().Should().Be("border-bottom-right-radius: 50rem");
        bottomRight.ToString().Should().Be("rounded-br-pill");
    }

    [Fact]
    public void BorderRadiusBuilder_Responsive_WorksCorrectly()
    {
        BorderRadiusBuilder responsive = BorderRadius.Lg.OnTablet;
        string classResult = responsive.ToClass();
        
        // Should contain both the border radius class and responsive breakpoint
        classResult.Should().Contain("lg");
        classResult.Should().Contain("sm"); // Tablet breakpoint maps to "sm" in Bootstrap
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
        classResult.Should().Contain("tl-sm");
        classResult.Should().Contain("br-lg");
        
        string styleResult = chained.ToStyle();
        styleResult.Should().Contain("border-top-left-radius: 0.25rem");
        styleResult.Should().Contain("border-bottom-right-radius: 0.5rem");
    }

    [Fact]
    public void OverflowBuilder_BasicValues_GeneratesCorrectOutput()
    {
        // Test basic overflow values
        OverflowBuilder auto = Overflow.Auto;
        auto.ToClass().Should().Be("overflow-auto");
        auto.ToStyle().Should().Be("overflow: auto");
        auto.ToString().Should().Be("overflow-auto");

        OverflowBuilder hidden = Overflow.Hidden;
        hidden.ToClass().Should().Be("overflow-hidden");
        hidden.ToStyle().Should().Be("overflow: hidden");
        hidden.ToString().Should().Be("overflow-hidden");

        OverflowBuilder visible = Overflow.Visible;
        visible.ToClass().Should().Be("overflow-visible");
        visible.ToStyle().Should().Be("overflow: visible");
        visible.ToString().Should().Be("overflow-visible");

        OverflowBuilder scroll = Overflow.Scroll;
        scroll.ToClass().Should().Be("overflow-scroll");
        scroll.ToStyle().Should().Be("overflow: scroll");
        scroll.ToString().Should().Be("overflow-scroll");
    }

    [Fact]
    public void OverflowBuilder_GlobalKeywords_GeneratesCorrectOutput()
    {
        // Test global keywords (should generate styles only)
        OverflowBuilder inherit = Overflow.Inherit;
        inherit.ToClass().Should().Be("");
        inherit.ToStyle().Should().Be("overflow: inherit");
        inherit.ToString().Should().Be("");

        OverflowBuilder initial = Overflow.Initial;
        initial.ToClass().Should().Be("");
        initial.ToStyle().Should().Be("overflow: initial");
        initial.ToString().Should().Be("");

        OverflowBuilder revert = Overflow.Revert;
        revert.ToClass().Should().Be("");
        revert.ToStyle().Should().Be("overflow: revert");
        revert.ToString().Should().Be("");

        OverflowBuilder unset = Overflow.Unset;
        unset.ToClass().Should().Be("");
        unset.ToStyle().Should().Be("overflow: unset");
        unset.ToString().Should().Be("");
    }

    [Fact]
    public void OverflowBuilder_XAxis_GeneratesCorrectOutput()
    {
        // Test X-axis overflow
        OverflowBuilder xAuto = Overflow.X.Auto;
        xAuto.ToClass().Should().Be("overflow-x-auto");
        xAuto.ToStyle().Should().Be("overflow-x: auto");
        xAuto.ToString().Should().Be("overflow-x-auto");

        OverflowBuilder xHidden = Overflow.X.Hidden;
        xHidden.ToClass().Should().Be("overflow-x-hidden");
        xHidden.ToStyle().Should().Be("overflow-x: hidden");
        xHidden.ToString().Should().Be("overflow-x-hidden");

        OverflowBuilder xVisible = Overflow.X.Visible;
        xVisible.ToClass().Should().Be("overflow-x-visible");
        xVisible.ToStyle().Should().Be("overflow-x: visible");
        xVisible.ToString().Should().Be("overflow-x-visible");

        OverflowBuilder xScroll = Overflow.X.Scroll;
        xScroll.ToClass().Should().Be("overflow-x-scroll");
        xScroll.ToStyle().Should().Be("overflow-x: scroll");
        xScroll.ToString().Should().Be("overflow-x-scroll");
    }

    [Fact]
    public void OverflowBuilder_YAxis_GeneratesCorrectOutput()
    {
        // Test Y-axis overflow
        OverflowBuilder yAuto = Overflow.Y.Auto;
        yAuto.ToClass().Should().Be("overflow-y-auto");
        yAuto.ToStyle().Should().Be("overflow-y: auto");
        yAuto.ToString().Should().Be("overflow-y-auto");

        OverflowBuilder yHidden = Overflow.Y.Hidden;
        yHidden.ToClass().Should().Be("overflow-y-hidden");
        yHidden.ToStyle().Should().Be("overflow-y: hidden");
        yHidden.ToString().Should().Be("overflow-y-hidden");

        OverflowBuilder yVisible = Overflow.Y.Visible;
        yVisible.ToClass().Should().Be("overflow-y-visible");
        yVisible.ToStyle().Should().Be("overflow-y: visible");
        yVisible.ToString().Should().Be("overflow-y-visible");

        OverflowBuilder yScroll = Overflow.Y.Scroll;
        yScroll.ToClass().Should().Be("overflow-y-scroll");
        yScroll.ToStyle().Should().Be("overflow-y: scroll");
        yScroll.ToString().Should().Be("overflow-y-scroll");
    }

    [Fact]
    public void OverflowBuilder_AllAxis_GeneratesCorrectOutput()
    {
        // Test All axis (should be same as default)
        OverflowBuilder allAuto = Overflow.All.Auto;
        allAuto.ToClass().Should().Be("overflow-auto");
        allAuto.ToStyle().Should().Be("overflow: auto");
        allAuto.ToString().Should().Be("overflow-auto");

        OverflowBuilder allHidden = Overflow.All.Hidden;
        allHidden.ToClass().Should().Be("overflow-hidden");
        allHidden.ToStyle().Should().Be("overflow: hidden");
        allHidden.ToString().Should().Be("overflow-hidden");
    }

    [Fact]
    public void OverflowBuilder_Chaining_WorksCorrectly()
    {
        // Test chaining from value to axis
        OverflowBuilder valueToAxis = Overflow.Hidden.X;
        valueToAxis.ToClass().Should().Be("overflow-x-hidden");
        valueToAxis.ToStyle().Should().Be("overflow-x: hidden");

        OverflowBuilder valueToAxisY = Overflow.Auto.Y;
        valueToAxisY.ToClass().Should().Be("overflow-y-auto");
        valueToAxisY.ToStyle().Should().Be("overflow-y: auto");

        // Test chaining from axis to value
        OverflowBuilder axisToValue = Overflow.X.Hidden;
        axisToValue.ToClass().Should().Be("overflow-x-hidden");
        axisToValue.ToStyle().Should().Be("overflow-x: hidden");

        OverflowBuilder axisToValueY = Overflow.Y.Auto;
        axisToValueY.ToClass().Should().Be("overflow-y-auto");
        axisToValueY.ToStyle().Should().Be("overflow-y: auto");
    }

    [Fact]
    public void OverflowBuilder_AxisChaining_LastAxisWins()
    {
        // Test that last axis in chain wins
        OverflowBuilder xThenY = Overflow.X.Auto.Y.Hidden;
        xThenY.ToClass().Should().Be("overflow-y-hidden");
        xThenY.ToStyle().Should().Be("overflow-y: hidden");

        OverflowBuilder yThenX = Overflow.Y.Auto.X.Hidden;
        yThenX.ToClass().Should().Be("overflow-x-hidden");
        yThenX.ToStyle().Should().Be("overflow-x: hidden");

        OverflowBuilder allThenX = Overflow.All.Auto.X.Hidden;
        allThenX.ToClass().Should().Be("overflow-x-hidden");
        allThenX.ToStyle().Should().Be("overflow-x: hidden");
    }

    [Fact]
    public void OverflowBuilder_ValueChaining_LastValueWins()
    {
        // Test that last value in chain wins
        OverflowBuilder autoThenHidden = Overflow.Auto.Hidden;
        autoThenHidden.ToClass().Should().Be("overflow-hidden");
        autoThenHidden.ToStyle().Should().Be("overflow: hidden");

        OverflowBuilder hiddenThenScroll = Overflow.Hidden.Scroll;
        hiddenThenScroll.ToClass().Should().Be("overflow-scroll");
        hiddenThenScroll.ToStyle().Should().Be("overflow: scroll");

        // Test with axis
        OverflowBuilder xAutoThenHidden = Overflow.X.Auto.Hidden;
        xAutoThenHidden.ToClass().Should().Be("overflow-x-hidden");
        xAutoThenHidden.ToStyle().Should().Be("overflow-x: hidden");
    }

    [Fact]
    public void OverflowBuilder_Create_WorksCorrectly()
    {
        // Test the Create method
        OverflowBuilder created = OverflowBuilder.Create();
        created.ToClass().Should().Be("");
        created.ToStyle().Should().Be("");
        created.ToString().Should().Be("");

        // Test chaining from created builder
        OverflowBuilder createdWithX = OverflowBuilder.Create().X.Hidden;
        createdWithX.ToClass().Should().Be("overflow-x-hidden");
        createdWithX.ToStyle().Should().Be("overflow-x: hidden");

        OverflowBuilder createdWithY = OverflowBuilder.Create().Y.Auto;
        createdWithY.ToClass().Should().Be("overflow-y-auto");
        createdWithY.ToStyle().Should().Be("overflow-y: auto");
    }

    [Fact]
    public void OverflowBuilder_ComplexChaining_WorksCorrectly()
    {
        // Test complex chaining scenarios
        OverflowBuilder complex1 = OverflowBuilder.Create().X.Auto.Y.Hidden.All.Scroll;
        complex1.ToClass().Should().Be("overflow-scroll");
        complex1.ToStyle().Should().Be("overflow: scroll");

        OverflowBuilder complex2 = Overflow.Hidden.X.Auto.Y.Scroll;
        complex2.ToClass().Should().Be("overflow-y-scroll");
        complex2.ToStyle().Should().Be("overflow-y: scroll");

        OverflowBuilder complex3 = Overflow.X.Hidden.Y.Auto.All.Scroll;
        complex3.ToClass().Should().Be("overflow-scroll");
        complex3.ToStyle().Should().Be("overflow: scroll");
    }

    [Fact]
    public void OverflowBuilder_EmptyBuilder_ReturnsEmptyStrings()
    {
        // Test empty builder
        OverflowBuilder empty = OverflowBuilder.Create();
        empty.ToClass().Should().Be("");
        empty.ToStyle().Should().Be("");
        empty.ToString().Should().Be("");

        // Test empty builder with only axis
        OverflowBuilder emptyX = OverflowBuilder.Create().X;
        emptyX.ToClass().Should().Be("");
        emptyX.ToStyle().Should().Be("");
        emptyX.ToString().Should().Be("");
    }
}
