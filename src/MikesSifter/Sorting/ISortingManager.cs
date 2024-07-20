namespace MikesSifter.Sorting;

public interface ISortingManager
{
    IQueryable<TEntity> ApplySorting<TEntity>(IQueryable<TEntity> source, SortingOptions? sortingOptions);
}