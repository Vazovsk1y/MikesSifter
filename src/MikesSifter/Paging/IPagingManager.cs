namespace MikesSifter.Paging;

/// <summary>
/// Interface for applying paging.
/// </summary>
public interface IPagingManager
{
    /// <summary>
    /// Applies the specified paging options to the given data source.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entities in the data source.</typeparam>
    /// <param name="source">The data source to page.</param>
    /// <param name="pagingOptions">The paging options to apply.</param>
    /// <returns>A paged data source.</returns>
    IQueryable<TEntity> ApplyPaging<TEntity>(IQueryable<TEntity> source, PagingOptions? pagingOptions);
}