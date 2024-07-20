namespace MikesSifter.Filtering;

public interface IFilteringManager
{
    IQueryable<TEntity> ApplyFiltering<TEntity>(IQueryable<TEntity> source, FilteringOptions? filteringOptions);
}