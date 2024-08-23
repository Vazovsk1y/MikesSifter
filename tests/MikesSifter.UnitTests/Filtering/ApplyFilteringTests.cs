using FluentAssertions;
using MikesSifter.Exceptions;
using MikesSifter.Filtering;
using MikesSifter.UnitTests.Infrastructure;
using MikesSifter.UnitTests.Models;

namespace MikesSifter.UnitTests.Filtering;

public class ApplyFilteringTests
{
    private readonly IMikesSifter _sifter = new TestSifter();
    public static readonly TheoryData<FilteringTestCase> TestCases = FilteringData.TestCases;
    
    [Fact]
    public void ApplyFiltering_Should_Return_The_Same_Collection_WHEN_null_filtering_options_passed()
    {
        // arrange
        var expected = FilteringData.Entities.AsQueryable();

        // act
        var actual = _sifter.ApplyFiltering(expected, null);

        // assert
        actual.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void ApplyFiltering_Should_Return_The_Same_Collection_WHEN_filtering_options_with_empty_filters_passed()
    {
        // arrange
        var expected = FilteringData.Entities.AsQueryable();
        var filteringOptions = new FilteringOptions(Utils.PickRandom(Enum.GetValues<FilteringLogic>()), []);

        // act
        var actual = _sifter.ApplyFiltering(expected, filteringOptions);

        // assert
        actual.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void ApplyFiltering_Should_Throw_FilteringDisabledException_WHEN_try_to_filter_by_property_for_which_filtering_is_disabled()
    {
        // arrange
        var filteringOptions = new FilteringOptions(
            Utils.PickRandom(Enum.GetValues<FilteringLogic>()), 
            [new Filter(nameof(Entity.PropertyWithDisabledFiltering), Utils.PickRandom(Enum.GetValues<FilteringOperator>()), "no matter")]);
        
        
        // act
        void Action() => _sifter.ApplyFiltering(FilteringData.Entities.AsQueryable(), filteringOptions);
        
        // assert
        Assert.Throws<FilteringDisabledException>(Action);
    }
    
    [Fact]
    public void ApplyFiltering_Should_Throw_PropertyConfigurationNotFoundException_WHEN_try_to_filter_by_property_for_which_not_defined_configuration()
    {
        // arrange
        var filteringOptions = new FilteringOptions(
            Utils.PickRandom(Enum.GetValues<FilteringLogic>()), 
            [new Filter(nameof(Entity.PropertyWithNotDefinedConfiguration), Utils.PickRandom(Enum.GetValues<FilteringOperator>()), "no matter")]);
        
        // act
        void Action() => _sifter.ApplyFiltering(FilteringData.Entities.AsQueryable(), filteringOptions);
        
        // assert
        Assert.Throws<PropertyConfigurationNotFoundException>(Action);
    }
    
    [Theory]
    [MemberData(nameof(TestCases))]
    public void ApplyFiltering_Should_Return_Expected(FilteringTestCase testCase)
    {
        // arrange
        
        
        // act
        var actual = _sifter.ApplyFiltering(FilteringData.Entities.AsQueryable(), testCase.FilteringOptions);

        // assert
        actual.Should().BeEquivalentTo(testCase.Expected);
    }
}