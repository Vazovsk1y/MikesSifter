namespace MikesSifter.Sorting;

/// <summary>
/// Represents sorting options including a collection of sorters.
/// </summary>
/// <param name="Sorters">The collection of sorters to apply.</param>
public record SortingOptions(IReadOnlyCollection<Sorter> Sorters);