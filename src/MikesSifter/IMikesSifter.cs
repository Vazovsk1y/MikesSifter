using MikesSifter.Filtering;
using MikesSifter.Paging;
using MikesSifter.Sorting;

namespace MikesSifter;

public interface IMikesSifter : IFilteringManager, IPagingManager, ISortingManager
{
    IQueryable<TEntity> Apply<TEntity>(IQueryable<TEntity> source, IMikesSifterModel sifterModel);
}