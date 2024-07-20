using FluentAssertions;
using MikesSifter.Exceptions;
using MikesSifter.Paging;
using MikesSifter.UnitTests.Infrastructure;
using NSubstitute;

namespace MikesSifter.UnitTests.Paging;

public class ApplyTests
{
    private readonly IMikesSifter _sifter = new TestSifter();

    [Theory]
    [InlineData(0, 10)]
    [InlineData(10, 0)]
    [InlineData(-1, 10)]
    [InlineData(10, -1)]
    [InlineData(0, 0)]
    [InlineData(-1, -1)]
    public void Apply_Should_Throw_PagingException_WHEN_invalid_paging_options_passed(int pageIndex, int pageSize)
    {
        // Arrange
        var invalidPagingOptions = new PagingOptions(pageIndex, pageSize);
        var modelMock = Substitute.For<IMikesSifterModel>();
        modelMock.GetPagingOptions().Returns(invalidPagingOptions);

        // Act
        void Action() => _sifter.Apply(PagingData.Entities.AsQueryable(), modelMock);

        // Assert 
        Assert.Throws<PagingException>(Action);
    }
    
    [Fact]
    public void Apply_Should_Return_The_Same_Collection_WHEN_null_paging_options_passed()
    {
        // Arrange
        var expected = PagingData.Entities.AsQueryable();
        var modelMock = Substitute.For<IMikesSifterModel>();
        modelMock.GetPagingOptions().Returns((PagingOptions?)null);

        // Act
        var actual = _sifter.Apply(expected, modelMock);

        // Assert 
        actual.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void Apply_Should_Return_Paged_Collection_WHEN_valid_paging_options_passed()
    {
        // Arrange
        const int pageIndex = 1;
        const int pageSize = 2;

        var pagingOptions = new PagingOptions(pageIndex, pageSize);
        var modelMock = Substitute.For<IMikesSifterModel>();
        modelMock.GetPagingOptions().Returns(pagingOptions);
        
        var expected = new[]
        {
            PagingData.Entities[0],
            PagingData.Entities[1]
        };

        // Act
        var actual = _sifter.Apply(PagingData.Entities.AsQueryable(), modelMock);

        // Assert 
        actual.Should().BeEquivalentTo(expected);
    }
}