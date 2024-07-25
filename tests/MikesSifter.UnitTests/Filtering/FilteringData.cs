using System.Text.Json;
using MikesSifter.Filtering;
using MikesSifter.UnitTests.Infrastructure;
using MikesSifter.UnitTests.Models;

namespace MikesSifter.UnitTests.Filtering;

public static class FilteringData
{
    public static readonly IReadOnlyList<Entity> Entities = new List<Entity>()
    {
        new ()
        {
            PropertyWithDisabledFiltering = 5,
            PropertyWithDisabledSorting = "Cool",
            Uint = 15,
            String = "CEntity1",
            Bool = true,
            NullableInt32 = null,
            DateTimeOffset = DateTimeOffset.Parse("2024-09-22T13:00:00Z"),
            ComplexType = new ComplexType("BTitle1", "BValue1"),
            RelatedCollection = new List<ComplexType>
            {
                new("RelatedTitle1", "RelatedValue1"),
                new("RelatedTitle2", "RelatedValue2"),
                new("RelatedTitle42ff2", "RelatedValue41100")
            }
        },
        new ()
        {
            PropertyWithDisabledFiltering = 10,
            PropertyWithDisabledSorting = "Awesome",
            Uint = 10,
            String = "AEntity2",
            Bool = false,
            NullableInt32 = 200,
            DateTimeOffset = DateTimeOffset.Parse("2024-03-22T14:00:00Z"),
            ComplexType = new ComplexType("ATitle2", "CValue2"),
            RelatedCollection = new List<ComplexType>
            {
                new("RelatedTitle3", "RelatedValue3"),
                new("RelatedTitle4", "RelatedValue4"),
                new("RelatedTitle422", "RelatedValue411"),
                new("RelatedTitle422aa", "RelatedValue411bb")
            }
        },
        new ()
        {
            PropertyWithDisabledFiltering = 15,
            PropertyWithDisabledSorting = "Great",
            Uint = 30,
            String = "BEntity3",
            Bool = true,
            NullableInt32 = 100,
            DateTimeOffset = DateTimeOffset.Parse("2024-11-22T15:00:00Z"),
            ComplexType = new ComplexType("CTitle3", "AValue3"),
            RelatedCollection = new List<ComplexType>
            {
                new("RelatedTitle5", "RelatedValue5"),
            }
        },
        new ()
        {
            PropertyWithDisabledFiltering = 88,
            PropertyWithDisabledSorting = "Poor",
            Uint = 9990,
            String = "BEntity3",
            Bool = false,
            NullableInt32 = 3,
            DateTimeOffset = DateTimeOffset.Parse("2024-01-22T19:00:00Z"),
            ComplexType = new ComplexType("Pipe_CTitle3", "Term_AValue3"),
            RelatedCollection = new List<ComplexType>
            {
                new("RelatedTitle577", "RelatedValue888"),
            }
        },
    };
    
    public static readonly TheoryData<FilteringTestCase> TestCases = new()
    {
        new FilteringTestCase  (new FilteringOptions(Utils.PickRandom(Enum.GetValues<FilteringLogic>()), new List<Filter> { new(nameof(Entity.Uint), FilteringOperators.Equal, "15") }), 
            new List<Entity> { Entities[0] }),
        new FilteringTestCase (new FilteringOptions(Utils.PickRandom(Enum.GetValues<FilteringLogic>()), new List<Filter> { new(nameof(Entity.String), FilteringOperators.StartsWith, "A") }), 
            new List<Entity> { Entities[1] }),

        // Multiple filters with AND logic
        new FilteringTestCase (new FilteringOptions(FilteringLogic.And, new List<Filter>
            {
                new(nameof(Entity.Bool), FilteringOperators.Equal, "true"),
                new(nameof(Entity.DateTimeOffset), FilteringOperators.LessThan, "2024-10-01T00:00:00Z")
            }), 
            new List<Entity> { Entities[0] }),

        // Multiple filters with OR logic
        new FilteringTestCase (new FilteringOptions(FilteringLogic.Or, new List<Filter>
            {
                new(nameof(Entity.Uint), FilteringOperators.Equal, "10"),
                new(nameof(Entity.NullableInt32), FilteringOperators.GreaterThan, "50")
            }), 
            new List<Entity> { Entities[1], Entities[2] }),

        // Filters with complex type property
        new FilteringTestCase (new FilteringOptions(Utils.PickRandom(Enum.GetValues<FilteringLogic>()), new List<Filter> 
            { new("ComplexType_title", FilteringOperators.Equal, "BTitle1") }), 
            new List<Entity> { Entities[0] }),

        // Filters with related collection property
        new FilteringTestCase (new FilteringOptions(Utils.PickRandom(Enum.GetValues<FilteringLogic>()), new List<Filter> 
            { new("RelatedCollectionCount", FilteringOperators.GreaterThanOrEqual, "3") }), 
            new List<Entity> { Entities[0], Entities[1] }),
        
        // Custom filters
        new FilteringTestCase (new FilteringOptions(Utils.PickRandom(Enum.GetValues<FilteringLogic>()), new List<Filter>
            {
                new(nameof(Entity.ComplexType), FilteringOperators.Equal, JsonSerializer.Serialize(new ComplexType("BTitle1", "BValue1")))
            }), 
            new List<Entity> { Entities[0] }),
        new FilteringTestCase (new FilteringOptions(Utils.PickRandom(Enum.GetValues<FilteringLogic>()), new List<Filter>
            {
                new(nameof(Entity.RelatedCollection), FilteringOperators.Contains, JsonSerializer.Serialize(new ComplexType("RelatedTitle1", "RelatedValue1")))
            }), 
            new List<Entity> { Entities[0] }),
        
        // Custom filters combines with built by sifter
        new FilteringTestCase (new FilteringOptions(FilteringLogic.And, new List<Filter>
        {
            new(nameof(Entity.ComplexType), FilteringOperators.Equal, JsonSerializer.Serialize(new ComplexType("BTitle1", "BValue1"))),
            new(nameof(Entity.Bool), FilteringOperators.Equal, "true")
        }), 
            new List<Entity> { Entities[0] }),
        new FilteringTestCase (new FilteringOptions(FilteringLogic.Or, new List<Filter>
            {
                new(nameof(Entity.RelatedCollection), FilteringOperators.Contains, JsonSerializer.Serialize(new ComplexType("RelatedTitle1", "RelatedValue1"))),
                new(nameof(Entity.String), FilteringOperators.NotEqual, "AEntity2")
            }), 
            new List<Entity> { Entities[0], Entities[2], Entities[3] })
    };
}