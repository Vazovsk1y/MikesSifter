using FluentAssertions;
using MikesSifter.Exceptions;
using MikesSifter.Filtering;
using MikesSifter.UnitTests.Infrastructure;
using MikesSifter.UnitTests.Models;
using NSubstitute;

namespace MikesSifter.UnitTests.Filtering;

public class ApplyTests
{
    private readonly IMikesSifter _sifter = new TestSifter();
    public static readonly TheoryData<FilteringTestCase> TestCases = FilteringData.TestCases;
    
    [Fact]
    public void Apply_Should_Return_The_Same_Collection_WHEN_null_filtering_options_passed()
    {
        // arrange
        var expected = FilteringData.Entities.AsQueryable();
        var modelMock = Substitute.For<IMikesSifterModel>();
        modelMock.GetFilteringOptions().Returns((FilteringOptions?)null);

        // act
        var actual = _sifter.Apply(expected, modelMock);

        // assert
        actual.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void Apply_Should_Return_The_Same_Collection_WHEN_filtering_options_with_empty_filters_passed()
    {
        // arrange
        var expected = FilteringData.Entities.AsQueryable();
        var modelMock = Substitute.For<IMikesSifterModel>();
        var filteringOptions = new FilteringOptions(Utils.PickRandom(Enum.GetValues<FilteringLogic>()), []); 
        modelMock.GetFilteringOptions().Returns(filteringOptions);

        // act
        var actual = _sifter.Apply(expected, modelMock);

        // assert
        actual.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void Apply_Should_Throw_FilteringDisabledException_WHEN_try_to_filter_by_property_for_which_filtering_is_disabled()
    {
        // arrange
        var modelMock = Substitute.For<IMikesSifterModel>();
        var filteringOptions = new FilteringOptions(
            Utils.PickRandom(Enum.GetValues<FilteringLogic>()), 
            [new Filter(nameof(Entity.PropertyWithDisabledFiltering), Utils.PickRandom(Enum.GetValues<FilteringOperator>()), "no matter")]);
        
        modelMock.GetFilteringOptions().Returns(filteringOptions);
        
        // act
        void Action() => _sifter.Apply(FilteringData.Entities.AsQueryable(), modelMock);
        
        // assert
        Assert.Throws<FilteringDisabledException>(Action);
    }
    
    [Fact]
    public void Apply_Should_Throw_PropertyConfigurationNotFoundException_WHEN_try_to_filter_by_property_for_which_not_defined_configuration()
    {
        // arrange
        var modelMock = Substitute.For<IMikesSifterModel>();
        var filteringOptions = new FilteringOptions(
            Utils.PickRandom(Enum.GetValues<FilteringLogic>()), 
            [new Filter(nameof(Entity.PropertyWithNotDefinedConfiguration), Utils.PickRandom(Enum.GetValues<FilteringOperator>()), "no matter")]);
        modelMock.GetFilteringOptions().Returns(filteringOptions);
        
        // act
        void Action() => _sifter.Apply(FilteringData.Entities.AsQueryable(), modelMock);
        
        // assert
        Assert.Throws<PropertyConfigurationNotFoundException>(Action);
    }
    
    [Theory]
    [MemberData(nameof(TestCases))]
    public void Apply_Should_Return_Expected(FilteringTestCase testCase)
    {
        // arrange
        var modelMock = Substitute.For<IMikesSifterModel>();
        modelMock.GetFilteringOptions().Returns(testCase.FilteringOptions);
        
        // act
        var actual = _sifter.Apply(FilteringData.Entities.AsQueryable(), modelMock);

        // assert
        actual.Should().BeEquivalentTo(testCase.Expected);
    }
}