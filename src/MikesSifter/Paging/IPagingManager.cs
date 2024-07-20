namespace MikesSifter.Paging;

public interface IPagingManager
{
    IQueryable<TEntity> ApplyPaging<TEntity>(IQueryable<TEntity> source, PagingOptions? pagingOptions);
}