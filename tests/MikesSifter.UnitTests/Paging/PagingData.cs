using MikesSifter.UnitTests.Models;

namespace MikesSifter.UnitTests.Paging;

public static class PagingData
{
    public static readonly IReadOnlyList<Entity> Entities = new List<Entity>()
    {
        new()
        {
            PropertyWithDisabledFiltering = 5,
            PropertyWithDisabledSorting = "Cool",
            Uint = 10,
            String = "Entity1",
            Bool = true,
            NullableInt32 = null,
            DateTimeOffset = DateTimeOffset.Parse("2024-05-22T13:00:00Z"),
            ComplexType = new ComplexType("Title1", "Value1"),
            RelatedCollection = new List<ComplexType>
            {
                new("RelatedTitle1", "RelatedValue1"),
                new("RelatedTitle2", "RelatedValue2")
            }
        },
        new()
        {
            PropertyWithDisabledFiltering = 10,
            PropertyWithDisabledSorting = "Awesome",
            Uint = 20,
            String = "Entity2",
            Bool = false,
            NullableInt32 = 100,
            DateTimeOffset = DateTimeOffset.Parse("2024-06-22T14:00:00Z"),
            ComplexType = new ComplexType("Title2", "Value2"),
            RelatedCollection = new List<ComplexType>
            {
                new("RelatedTitle3", "RelatedValue3"),
                new("RelatedTitle4", "RelatedValue4")
            }
        },
        new()
        {
            PropertyWithDisabledFiltering = 15,
            PropertyWithDisabledSorting = "Great",
            Uint = 30,
            String = "Entity3",
            Bool = true,
            NullableInt32 = 200,
            DateTimeOffset = DateTimeOffset.Parse("2024-07-22T15:00:00Z"),
            ComplexType = new ComplexType("Title3", "Value3"),
            RelatedCollection = new List<ComplexType>
            {
                new("RelatedTitle5", "RelatedValue5"),
                new("RelatedTitle6", "RelatedValue6")
            }
        },
        new()
        {
            PropertyWithDisabledFiltering = 20,
            PropertyWithDisabledSorting = "Amazing",
            Uint = 40,
            String = "Entity4",
            Bool = false,
            NullableInt32 = 300,
            DateTimeOffset = DateTimeOffset.Parse("2024-08-22T16:00:00Z"),
            ComplexType = new ComplexType("Title4", "Value4"),
            RelatedCollection = new List<ComplexType>
            {
                new("RelatedTitle7", "RelatedValue7"),
                new("RelatedTitle8", "RelatedValue8")
            }
        }
    };
}