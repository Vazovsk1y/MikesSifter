namespace MikesSifter.Sorting;

/// <summary>
/// Interface for applying sorting to a queryable data source.
/// </summary>
public interface ISortingManager
{
    /// <summary>
    /// Applies the specified sorting options to the given queryable data source.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entities in the data source.</typeparam>
    /// <param name="source">The queryable data source to sort.</param>
    /// <param name="sortingOptions">The sorting options to apply.</param>
    /// <returns>A sorted queryable data source.</returns>
    IQueryable<TEntity> ApplySorting<TEntity>(IQueryable<TEntity> source, SortingOptions? sortingOptions);
}