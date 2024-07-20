using FluentAssertions;
using MikesSifter.Exceptions;
using MikesSifter.Paging;
using MikesSifter.UnitTests.Infrastructure;

namespace MikesSifter.UnitTests.Paging;

public class ApplyPagingTests
{
    private readonly IMikesSifter _sifter = new TestSifter();

    [Theory]
    [InlineData(0, 10)]
    [InlineData(10, 0)]
    [InlineData(-1, 10)]
    [InlineData(10, -1)]
    [InlineData(0, 0)]
    [InlineData(-1, -1)]
    public void ApplyPaging_Should_Throw_PagingException_WHEN_invalid_paging_options_passed(int pageIndex, int pageSize)
    {
        // Arrange
        var invalidPagingOptions = new PagingOptions(pageIndex, pageSize);

        // Act
        void Action() => _sifter.ApplyPaging(PagingData.Entities.AsQueryable(), invalidPagingOptions);

        // Assert 
        Assert.Throws<PagingException>(Action);
    }
    
    [Fact]
    public void ApplyPaging_Should_Return_The_Same_Collection_WHEN_null_paging_options_passed()
    {
        // Arrange
        var expected = PagingData.Entities.AsQueryable();

        // Act
        var actual = _sifter.ApplyPaging(expected, null);

        // Assert 
        actual.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void ApplyPaging_Should_Return_Paged_Collection_WHEN_valid_paging_options_passed()
    {
        // Arrange
        const int pageIndex = 1;
        const int pageSize = 2;

        var pagingOptions = new PagingOptions(pageIndex, pageSize);
        var expected = new[]
        {
            PagingData.Entities[0],
            PagingData.Entities[1]
        };

        // Act
        var actual = _sifter.ApplyPaging(PagingData.Entities.AsQueryable(), pagingOptions);

        // Assert 
        actual.Should().BeEquivalentTo(expected);
    }
}