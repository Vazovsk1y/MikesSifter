namespace MikesSifter.Sorting;

/// <summary>
/// Interface for applying sorting.
/// </summary>
public interface ISortingManager
{
    /// <summary>
    /// Applies the specified sorting options to the given data source.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entities in the data source.</typeparam>
    /// <param name="source">The data source to sort.</param>
    /// <param name="sortingOptions">The sorting options to apply.</param>
    /// <returns>A sorted data source.</returns>
    IQueryable<TEntity> ApplySorting<TEntity>(IQueryable<TEntity> source, SortingOptions? sortingOptions);
}