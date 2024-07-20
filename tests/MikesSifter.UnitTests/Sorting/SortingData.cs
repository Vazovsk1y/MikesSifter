using MikesSifter.Sorting;
using MikesSifter.UnitTests.Models;

namespace MikesSifter.UnitTests.Sorting;

public static class SortingData
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
    };

    public static readonly TheoryData<(SortingOptions sortingOptions, IEnumerable<Entity> expected)> ValidSortingOptions = new()
    {
        (new SortingOptions(new List<Sorter> { new(0, nameof(Entity.Uint), true) }), 
                new List<Entity> { Entities[1], Entities[0], Entities[2] }),
            (new SortingOptions(new List<Sorter> { new(0, nameof(Entity.Uint), false) }), 
                new List<Entity> { Entities[2], Entities[0], Entities[1] }),
            
            // Nullable property type.
            (new SortingOptions(new List<Sorter> { new(0, nameof(Entity.NullableInt32), true) }), 
                new List<Entity> { Entities[0], Entities[2], Entities[1] }),
            (new SortingOptions(new List<Sorter> { new(0, nameof(Entity.NullableInt32), false) }), 
                new List<Entity> { Entities[1], Entities[2], Entities[0] }),
            
            // By complex-type property with custom alias.
            (new SortingOptions(new List<Sorter> { new(0, "ComplexType_title", true) }), 
                new List<Entity> { Entities[1], Entities[0], Entities[2] }),
            (new SortingOptions(new List<Sorter> { new(0, "ComplexType_title", false) }), 
                new List<Entity> { Entities[2], Entities[0], Entities[1] }),
            
            // By complex-type property without custom alias.
            (new SortingOptions(new List<Sorter> { new(0, $"{nameof(ComplexType)}.{nameof(ComplexType.Value)}", true) }), 
                new List<Entity> { Entities[2], Entities[0], Entities[1] }),
            (new SortingOptions(new List<Sorter> { new(0, $"{nameof(ComplexType)}.{nameof(ComplexType.Value)}", false) }), 
                new List<Entity> { Entities[1], Entities[0], Entities[2] }),
            
            // By related collection property with custom alias.
            (new SortingOptions(new List<Sorter> { new(0, "RelatedCollectionCount", true) }), 
                new List<Entity> { Entities[2], Entities[0], Entities[1] }),
            (new SortingOptions(new List<Sorter> { new(0, "RelatedCollectionCount", false) }), 
                new List<Entity> { Entities[1], Entities[0], Entities[2] }),
            
            // Multiple sorters (thenBy, thenByDescending).
            (new SortingOptions(new List<Sorter> 
            { 
                new(0, nameof(Entity.PropertyWithDisabledFiltering), true), 
                new(1, nameof(Entity.Bool), true) 
            }), 
                new List<Entity> { Entities[0], Entities[1], Entities[2] }),
            (new SortingOptions(new List<Sorter> 
            { 
                new(0, nameof(Entity.PropertyWithDisabledFiltering), true), 
                new(1, nameof(Entity.Bool), false) 
            }), 
                new List<Entity> { Entities[1], Entities[0], Entities[2] }),
            (new SortingOptions(new List<Sorter> 
            { 
                new(0, nameof(Entity.PropertyWithDisabledFiltering), false), 
                new(1, nameof(Entity.Bool), true) 
            }), 
                new List<Entity> { Entities[2], Entities[1], Entities[0] }),
            (new SortingOptions(new List<Sorter> 
            { 
                new(0, nameof(Entity.PropertyWithDisabledFiltering), false), 
                new(1, nameof(Entity.Bool), false) 
            }), 
                new List<Entity> { Entities[2], Entities[1], Entities[0] }),

            (new SortingOptions(new List<Sorter> 
            { 
                new(0, nameof(Entity.DateTimeOffset), true), 
                new(1, nameof(Entity.String), true) 
            }), 
                new List<Entity> { Entities[1], Entities[0], Entities[2] }),
            (new SortingOptions(new List<Sorter> 
            { 
                new(0, nameof(Entity.DateTimeOffset), true), 
                new(1, nameof(Entity.String), false) 
            }), 
                new List<Entity> { Entities[1], Entities[0], Entities[2] }),
            (new SortingOptions(new List<Sorter> 
            { 
                new(0, nameof(Entity.DateTimeOffset), false), 
                new(1, nameof(Entity.String), true) 
            }), 
                new List<Entity> { Entities[2], Entities[0], Entities[1] }),
            (new SortingOptions(new List<Sorter> 
            { 
                new(0, nameof(Entity.DateTimeOffset), false), 
                new(1, nameof(Entity.String), false) 
            }), 
                new List<Entity> { Entities[2], Entities[0], Entities[1] }),
    };
}