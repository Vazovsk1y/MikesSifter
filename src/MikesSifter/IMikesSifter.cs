using MikesSifter.Filtering;
using MikesSifter.Paging;
using MikesSifter.Sorting;

namespace MikesSifter;

/// <summary>
/// Interface that combines filtering, paging, and sorting capabilities.
/// </summary>
public interface IMikesSifter : IFilteringManager, IPagingManager, ISortingManager
{
    /// <summary>
    /// Applies filtering, paging, and sorting to the given data source based on the provided model.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entities in the data source.</typeparam>
    /// <param name="source">The data source to apply the operations to.</param>
    /// <param name="sifterModel">The model containing the filtering, paging, and sorting options.</param>
    /// <returns>A data source with the applied operations.</returns>
    IQueryable<TEntity> Apply<TEntity>(IQueryable<TEntity> source, IMikesSifterModel sifterModel);
}