using FluentAssertions;
using MikesSifter.Exceptions;
using MikesSifter.Sorting;
using MikesSifter.UnitTests.Infrastructure;
using MikesSifter.UnitTests.Models;
using NSubstitute;

namespace MikesSifter.UnitTests.Sorting;

public class ApplyTests
{
    private readonly IMikesSifter _sifter = new TestSifter();
    public static TheoryData<SortingTestCase> TestCases = SortingData.TestCases;
    
    [Fact]
    public void Apply_Should_Return_The_Same_Collection_WHEN_null_sorting_options_passed()
    {
        // arrange
        var expected = SortingData.Entities.AsQueryable();
        var modelMock = Substitute.For<IMikesSifterModel>();
        modelMock.GetSortingOptions().Returns((SortingOptions?)null);

        // act
        var actual = _sifter.Apply(expected, modelMock);

        // assert
        actual.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void Apply_Should_Return_The_Same_Collection_WHEN_sorting_options_with_empty_sorters_passed()
    {
        // arrange
        var expected = SortingData.Entities.AsQueryable();
        var modelMock = Substitute.For<IMikesSifterModel>();
        var sortingOptions = new SortingOptions([]);
        modelMock.GetSortingOptions().Returns(sortingOptions);

        // act
        var actual = _sifter.Apply(expected, modelMock);

        // assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Apply_Should_Throw_SortingDisabledException_WHEN_try_to_sort_by_property_for_which_sorting_is_disabled()
    {
        // arrange
        var modelMock = Substitute.For<IMikesSifterModel>();
        var sortingOptions = new SortingOptions([new Sorter(1, nameof(Entity.PropertyWithDisabledSorting), true)]);
        modelMock.GetSortingOptions().Returns(sortingOptions);
        
        // act
        void Action() => _sifter.Apply(SortingData.Entities.AsQueryable(), modelMock);
        
        // assert
        Assert.Throws<SortingDisabledException>(Action);
    }
    
    [Theory]
    [MemberData(nameof(TestCases))]
    public void Apply_Should_Return_Expected_Collection_WHEN_valid_sorting_options_passed(SortingTestCase testCase)
    {
        // arrange
        var modelMock = Substitute.For<IMikesSifterModel>();
        modelMock.GetSortingOptions().Returns(testCase.SortingOptions);
        
        // act
        var actual = _sifter.Apply(SortingData.Entities.AsQueryable(), modelMock);
        
        // assert
        actual.Should().BeEquivalentTo(testCase.Expected);
    }

    [Fact]
    public void Apply_Should_Throw_PropertyConfigurationNotFoundException_WHEN_try_to_sort_by_property_for_which_not_defined_configuration()
    {
        // arrange
        var modelMock = Substitute.For<IMikesSifterModel>();
        var sortingOptions = new SortingOptions([ new Sorter(0, nameof(Entity.PropertyWithNotDefinedConfiguration), true) ]);
        modelMock.GetSortingOptions().Returns(sortingOptions);
        
        // act
        void Action() => _sifter.Apply(SortingData.Entities.AsQueryable(), modelMock);
        
        // assert
        Assert.Throws<PropertyConfigurationNotFoundException>(Action);
    }
}