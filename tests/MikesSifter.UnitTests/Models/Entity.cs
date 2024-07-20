namespace MikesSifter.UnitTests.Models;

public class Entity
{
    public string? PropertyWithNotDefinedConfiguration { get; init; }
    
    public required int PropertyWithDisabledFiltering { get; init; }
    
    public required string PropertyWithDisabledSorting { get; init; }
    
    public required uint Uint { get; init; }

    public required string String { get; init; }
    
    public required bool Bool { get; init; }
    
    public int? NullableInt32 { get; init; }

    public required DateTimeOffset DateTimeOffset { get; init; }

    public required ComplexType ComplexType { get; init; }
    
    public required ICollection<ComplexType> RelatedCollection { get; init; }
}