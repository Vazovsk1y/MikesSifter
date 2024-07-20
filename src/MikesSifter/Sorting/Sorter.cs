namespace MikesSifter.Sorting;

/// <summary>
/// Represents a sorting criterion for a specific property.
/// </summary>
/// <param name="Order">The order in which to apply the sorter.</param>
/// <param name="PropertyAlias">The alias of the property to sort by.</param>
/// <param name="Ascending">Indicates whether to sort in ascending order.</param>
public record Sorter(
    int Order, 
    string PropertyAlias, 
    bool Ascending);