﻿namespace MikesSifter.Filtering;

/// <summary>
/// Interface for applying filtering to a queryable data source.
/// </summary>
public interface IFilteringManager
{
    /// <summary>
    /// Applies the specified filtering options to the given queryable data source.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entities in the data source.</typeparam>
    /// <param name="source">The queryable data source to filter.</param>
    /// <param name="filteringOptions">The filtering options to apply.</param>
    /// <returns>A filtered queryable data source.</returns>
    IQueryable<TEntity> ApplyFiltering<TEntity>(IQueryable<TEntity> source, FilteringOptions? filteringOptions);
}
