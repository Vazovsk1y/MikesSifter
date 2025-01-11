using System.Reflection;
using MikesSifter.Filtering;

namespace MikesSifter;

public class MikesSifterPropertyBuilder<TEntity>
{
    private readonly PropertyInfo _propertyInfo;
    private readonly string _targetPropertyPath;

    private CustomFiltersBuilder<TEntity>? _customFiltersBuilder;
    private bool _isFilterable;
    private bool _isSortable;
    private string _alias;
    
    internal MikesSifterPropertyBuilder(PropertyInfo propertyInfo, string targetPropertyPath)
    {
        ArgumentNullException.ThrowIfNull(propertyInfo);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetPropertyPath);

        _targetPropertyPath = targetPropertyPath;
        _propertyInfo = propertyInfo;
        _alias = _targetPropertyPath;
    }

    /// <summary>
    /// Enables filtering for the property.
    /// </summary>
    /// <returns>The current <see cref="MikesSifterPropertyBuilder{TEntity}"/> instance.</returns>
    public MikesSifterPropertyBuilder<TEntity> EnableFiltering()
    {
        _isFilterable = true;
        return this;
    }

    /// <summary>
    /// Enables sorting for the property.
    /// </summary>
    /// <returns>The current <see cref="MikesSifterPropertyBuilder{TEntity}"/> instance.</returns>
    public MikesSifterPropertyBuilder<TEntity> EnableSorting()
    {
        _isSortable = true;
        return this;
    }

    /// <summary>
    /// Sets an alias for the property.
    /// </summary>
    /// <param name="alias">The alias to set for the property.</param>
    /// <returns>The current <see cref="MikesSifterPropertyBuilder{TEntity}"/> instance.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="alias"/> is null or whitespace.</exception>
    public MikesSifterPropertyBuilder<TEntity> HasAlias(string alias)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(alias);
    
        _alias = alias;
        return this;
    }

    /// <summary>
    /// Adds a custom filter for the property with the specified filtering operator.
    /// </summary>
    /// <param name="action">Action to configure custom filters.</param>
    /// <returns>The current <see cref="MikesSifterPropertyBuilder{TEntity}"/> instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="action"/> is null.</exception>
    public MikesSifterPropertyBuilder<TEntity> HasCustomFilters(Action<CustomFiltersBuilder<TEntity>> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        var builder = new CustomFiltersBuilder<TEntity>();
        action.Invoke(builder);
        _customFiltersBuilder = builder;
        
        return this;
    }

    internal PropertyConfiguration Build()
    {
        return new PropertyConfiguration(
            _propertyInfo,
            _alias,
            _targetPropertyPath,
            _customFiltersBuilder?.Build() ?? [],
            _isFilterable,
            _isSortable);
    }
}