using FluentAssertions;
using MikesSifter.Exceptions;
using MikesSifter.Sorting;
using MikesSifter.UnitTests.Infrastructure;
using MikesSifter.UnitTests.Models;
using NSubstitute;

namespace MikesSifter.UnitTests.Sorting;

public class ApplySortingTests
{
    private readonly IMikesSifter _sifter = new TestSifter();
    public static TheoryData<(SortingOptions sortingOptions, IEnumerable<Entity> expected)> ValidSortingOptions = SortingData.ValidSortingOptions;
    
    [Fact]
    public void ApplySorting_Should_Return_The_Same_Collection_WHEN_null_sorting_options_passed()
    {
        // arrange
        var expected = SortingData.Entities.AsQueryable();

        // act
        var actual = _sifter.ApplySorting(expected, null);

        // assert
        actual.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void ApplySorting_Should_Return_The_Same_Collection_WHEN_sorting_options_with_empty_sorters_passed()
    {
        // arrange
        var expected = SortingData.Entities.AsQueryable();
        var sortingOptions = new SortingOptions([]);

        // act
        var actual = _sifter.ApplySorting(expected, sortingOptions);

        // assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ApplySorting_Should_Throw_SortingDisabledException_WHEN_try_to_sort_by_property_for_which_sorting_is_disabled()
    {
        // arrange
        var modelMock = Substitute.For<IMikesSifterModel>();
        var sortingOptions = new SortingOptions([new Sorter(1, nameof(Entity.PropertyWithDisabledSorting), true)]);
        
        // act
        void Action() => _sifter.ApplySorting(SortingData.Entities.AsQueryable(), sortingOptions);
        
        // assert
        Assert.Throws<SortingDisabledException>(Action);
    }
    
    [Theory]
    [MemberData(nameof(ValidSortingOptions))]
    public void ApplySorting_Should_Return_Expected_Collection_WHEN_valid_sorting_options_passed((SortingOptions sortingOptions, IEnumerable<Entity> expected) param)
    {
        // arrange

        
        // act
        var actual = _sifter.ApplySorting(SortingData.Entities.AsQueryable(), param.sortingOptions);
        
        // assert
        actual.Should().BeEquivalentTo(param.expected);
    }

    [Fact]
    public void ApplySorting_Should_Throw_PropertyConfigurationNotFoundException_WHEN_try_to_sort_by_property_for_which_not_defined_configuration()
    {
        // arrange
        var sortingOptions = new SortingOptions([ new Sorter(0, nameof(Entity.PropertyWithNotDefinedConfiguration), true) ]);
        
        // act
        void Action() => _sifter.ApplySorting(SortingData.Entities.AsQueryable(), sortingOptions);
        
        // assert
        Assert.Throws<PropertyConfigurationNotFoundException>(Action);
    }
}