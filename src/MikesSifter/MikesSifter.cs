using System.ComponentModel;
using System.Linq.Expressions;
using MikesSifter.Exceptions;
using MikesSifter.Filtering;
using MikesSifter.Paging;
using MikesSifter.Sorting;

namespace MikesSifter;

public abstract class MikesSifter : IMikesSifter
{
    private const string FilteringParameterName = "x";
    private const string SortingParameterName = "e";
    private readonly IReadOnlyCollection<EntityConfiguration> _configurations;

    protected MikesSifter()
    {
        var builder = new MikesSifterBuilder();
        ConfigureInternal(builder);
        _configurations = builder.Build();
    }
    
    public virtual IQueryable<TEntity> Apply<TEntity>(IQueryable<TEntity> source, IMikesSifterModel sifterModel)
    {
        var result = source;
        try
        {
            var filteringOptions = sifterModel.GetFilteringOptions();
            if (filteringOptions is { Filters.Count: > 0 })
            {
                result = ApplyFilteringInternal(result, filteringOptions);
            }

            var sortingOptions = sifterModel.GetSortingOptions();
            if (sortingOptions is { Sorters.Count: > 0 })
            {
                result = ApplySortingInternal(result, sortingOptions);
            }

            var pagingOptions = sifterModel.GetPagingOptions();
            if (pagingOptions is not null)
            {
                result = ApplyPagingInternal(result, pagingOptions);
            }
            
            return result;
        }
        catch (Exception e) when (e is not MikesSifterException)
        {
            throw new MikesSifterException(e.Message, e);
        }
    }

    public virtual IQueryable<TEntity> ApplyFiltering<TEntity>(IQueryable<TEntity> source, FilteringOptions? filteringOptions)
    {
        try
        {
            return filteringOptions is null or { Filters.Count: 0 } ? source : ApplyFilteringInternal(source, filteringOptions);
        }
        catch (Exception e) when (e is not MikesSifterException)
        {
            throw new MikesSifterException(e.Message, e);
        }
    }
    
    public virtual IQueryable<TEntity> ApplySorting<TEntity>(IQueryable<TEntity> source, SortingOptions? sortingOptions)
    {
        try
        {
            return sortingOptions is null or { Sorters.Count: 0 } ? source : ApplySortingInternal(source, sortingOptions);
        }
        catch (Exception e) when (e is not MikesSifterException)
        {
            throw new MikesSifterException(e.Message, e);
        }
    }
    
    public virtual IQueryable<TEntity> ApplyPaging<TEntity>(IQueryable<TEntity> source, PagingOptions? pagingOptions)
    {
        try
        {
            return pagingOptions is null ? source : ApplyPagingInternal(source, pagingOptions);
        }
        catch (Exception e) when (e is not MikesSifterException)
        {
            throw new PagingException(e.Message, e);
        }
    }
    
    private IQueryable<T> ApplyFilteringInternal<T>(IQueryable<T> source, FilteringOptions filteringOptions)
    {
        Expression<Func<T, bool>> compositeFilterExpression = filteringOptions.Logic switch
        {
            FilteringLogic.And => GetAndFilterExpression<T>(filteringOptions.Filters),
            FilteringLogic.Or => GetOrFilterExpression<T>(filteringOptions.Filters),
            _ => throw new KeyNotFoundException($"Unsupported filtering logic [ \"{filteringOptions.Logic}\" ]."),
        };
        
        return source.Where(compositeFilterExpression);
    }
    
    private IQueryable<T> ApplySortingInternal<T>(IQueryable<T> source, SortingOptions sortingOptions)
    {
        var entityConfiguration = _configurations.FirstOrDefault(e => e.EntityType == typeof(T)); 
        if (entityConfiguration is null)
        {
            throw new EntityConfigurationNotFoundException(typeof(T));
        }

        var orderedSorters = sortingOptions
            .Sorters
            .OrderBy(e => e.Order)
            .ThenBy(e => e.PropertyAlias)
            .ThenBy(e => e.Ascending)
            .ToList();

        var result = source;
        var enumerator = orderedSorters.GetEnumerator();

        enumerator.MoveNext();
        var currentSorter = enumerator.Current;
        result = ApplySorter(result, currentSorter, false, entityConfiguration);

        while (enumerator.MoveNext())
        {
            currentSorter = enumerator.Current;
            result = ApplySorter(result, currentSorter, true, entityConfiguration);
        }

        return result;
    }
    
    private static IQueryable<T> ApplyPagingInternal<T>(IQueryable<T> source, PagingOptions pagingOptions)
    {
        if (pagingOptions.PageIndex <= 0)
        {
            throw PagingException.PageIndexMustBeGreaterThanZero();
        }

        if (pagingOptions.PageSize <= 0)
        {
            throw PagingException.PageSizeMustBeGreaterThanZero();
        }
        
        return source
            .Skip((pagingOptions.PageIndex - 1) * pagingOptions.PageSize)
            .Take(pagingOptions.PageSize);
    }
    
    private Expression<Func<T, bool>> GetOrFilterExpression<T>(IReadOnlyCollection<Filter> filters)
    {
        var parameter = Expression.Parameter(typeof(T), FilteringParameterName);
        Expression? orCompositeExpression = null;

        foreach (var filter in filters)
        {
            var filterExpression = BuildFilterExpression<T>(filter, parameter);
            orCompositeExpression = orCompositeExpression is null ? filterExpression : Expression.OrElse(orCompositeExpression, filterExpression);
        }

        ArgumentNullException.ThrowIfNull(orCompositeExpression);
        return Expression.Lambda<Func<T, bool>>(orCompositeExpression, parameter);
    }
    
    private Expression<Func<T, bool>> GetAndFilterExpression<T>(IReadOnlyCollection<Filter> filters)
    {
        var parameter = Expression.Parameter(typeof(T), FilteringParameterName);
        Expression? andCompositeExpression = null;

        foreach (var filter in filters)
        {
            var filterExpression = BuildFilterExpression<T>(filter, parameter);
            andCompositeExpression = andCompositeExpression is null ? filterExpression : Expression.AndAlso(andCompositeExpression, filterExpression);
        }

        ArgumentNullException.ThrowIfNull(andCompositeExpression);
        return Expression.Lambda<Func<T, bool>>(andCompositeExpression, parameter);
    }
    
    private Expression BuildFilterExpression<T>(Filter filter, ParameterExpression parameter)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filter.PropertyAlias);

        var entityConfiguration = _configurations.FirstOrDefault(e => e.EntityType == typeof(T));
        if (entityConfiguration is null)
        {
            throw new EntityConfigurationNotFoundException(typeof(T));
        }

        var propertyConfiguration = entityConfiguration.PropertyConfigurations.FirstOrDefault(e => e.PropertyAlias.Equals(filter.PropertyAlias, StringComparison.Ordinal));
        if (propertyConfiguration is null)
        {
            throw new PropertyConfigurationNotFoundException(typeof(T), filter.PropertyAlias);
        }

        if (!propertyConfiguration.IsFilterable)
        {
            throw new FilteringDisabledException(typeof(T), filter.PropertyAlias);
        }

        var customFilter = propertyConfiguration.CustomFilters.FirstOrDefault(e => e.FilteringOperator == filter.Operator);
        if (customFilter is not null)
        {
            var customFilterExpression = customFilter.ObtainFilterExpression.Invoke(customFilter.FilterValueConverter is null ? 
                filter.Value 
                : 
                customFilter.FilterValueConverter.Invoke(filter.Value));
            
            ArgumentNullException.ThrowIfNull(customFilterExpression);
            return Expression.Invoke(customFilterExpression, parameter);
        }

        var property = GetPropertyExpression(parameter, propertyConfiguration.TargetPropertyPath);
        var convertedProperty = Expression.Convert(property, property.Type);

        object? convertedFilterValue;
        if (filter.Value is not null)
        {
            convertedFilterValue = TypeDescriptor.GetConverter(convertedProperty.Type).ConvertFromInvariantString(filter.Value);
            if (convertedFilterValue is null)
            {
                throw new InvalidCastException($"Unable convert filter value [ \"{filter.Value}\" ] to [ \"{convertedProperty.Type.Name}\" ] property type.");
            }
        }
        else
        {
            convertedFilterValue = null;
        }
        
        var constant = Expression.Constant(convertedFilterValue);
        var convertedConstant = Expression.Convert(constant, convertedProperty.Type);

        return filter.Operator switch
        {
            FilteringOperator.Equal => Expression.Equal(convertedProperty, convertedConstant),
            FilteringOperator.NotEqual => Expression.NotEqual(convertedProperty, convertedConstant),
            FilteringOperator.LessThanOrEqual => Expression.LessThanOrEqual(convertedProperty, convertedConstant),
            FilteringOperator.GreaterThanOrEqual => Expression.GreaterThanOrEqual(convertedProperty, convertedConstant),
            FilteringOperator.LessThan => Expression.LessThan(convertedProperty, convertedConstant),
            FilteringOperator.GreaterThan => Expression.GreaterThan(convertedProperty, convertedConstant),
            FilteringOperator.Contains => Expression.Call(
                convertedProperty,
                typeof(string).GetMethod(nameof(string.Contains), [typeof(string)])!,
                convertedConstant),
            FilteringOperator.StartsWith => Expression.Call(
                convertedProperty,
                typeof(string).GetMethod(nameof(string.StartsWith), [typeof(string)])!,
                convertedConstant),
            _ => throw new KeyNotFoundException($"Unsupported filtering @operator [ \"{filter.Operator}\" ].")
        };
    }

    private static IQueryable<T> ApplySorter<T>(IQueryable<T> source, Sorter sorter, bool thenBy, EntityConfiguration configuration)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sorter.PropertyAlias);

        var propertyConfiguration = configuration.PropertyConfigurations.FirstOrDefault(e => e.PropertyAlias.Equals(sorter.PropertyAlias, StringComparison.Ordinal));
        if (propertyConfiguration is null)
        {
            throw new PropertyConfigurationNotFoundException(typeof(T), sorter.PropertyAlias);
        }

        if (!propertyConfiguration.IsSortable)
        {
            throw new SortingDisabledException(typeof(T), sorter.PropertyAlias);
        }
        
        var parameter = Expression.Parameter(typeof(T), SortingParameterName);
        var property = GetPropertyExpression(parameter, propertyConfiguration.TargetPropertyPath);
        var convertedProperty = Expression.Convert(property, typeof(object));
        var lambda = Expression.Lambda<Func<T, object>>(convertedProperty, parameter);

        if (sorter.Ascending)
        {
            return thenBy ? ((IOrderedQueryable<T>)source).ThenBy(lambda) : source.OrderBy(lambda);
        }

        return thenBy ? ((IOrderedQueryable<T>)source).ThenByDescending(lambda) : source.OrderByDescending(lambda);
    }
    
    private static Expression GetPropertyExpression(Expression parameter, string targetPropertyPath)
    {
        var names = targetPropertyPath.Split('.');
        return names.Aggregate(parameter, Expression.PropertyOrField);
    }

    private void ConfigureInternal(MikesSifterBuilder builder) => Configure(builder);
    
    protected abstract void Configure(MikesSifterBuilder builder);
}