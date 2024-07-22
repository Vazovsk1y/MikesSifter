using System.Linq.Expressions;
using System.Reflection;
using MikesSifter.Filtering;

namespace MikesSifter;

public abstract class MikesSifterEntityBuilder
{
    internal abstract MikesSifterPropertyConfiguration? FindConfiguration(string alias);
}

public sealed class MikesSifterEntityBuilder<TEntity> : MikesSifterEntityBuilder
{
    private readonly Dictionary<PropertyInfo, MikesSifterPropertyConfiguration> _configurations = new();
    
    /// <summary>
    /// Configures a property of the entity type using the specified property expression.
    /// </summary>
    /// <param name="expression">An expression that identifies the property to configure.</param>
    /// <returns>A <see cref="MikesSifterPropertyBuilder"/> to further configure the property.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="expression"/> is null.</exception>
    public MikesSifterPropertyBuilder Property(Expression<Func<TEntity, object?>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);
        return new MikesSifterPropertyBuilder(this, expression);
    }

    public class MikesSifterPropertyBuilder
    {
        private readonly Dictionary<FilteringOperators, Func<string?, Expression>> _customFilters = [];
        private readonly MikesSifterEntityBuilder<TEntity> _entityBuilder;
        private readonly PropertyInfo _propertyInfo;
        private readonly string _fullName;

        private bool _isFilterable;
        private bool _isSortable;
        private string _alias;
        public MikesSifterPropertyBuilder(MikesSifterEntityBuilder<TEntity> entityBuilder, Expression<Func<TEntity, object?>> expression)
        {
            ArgumentNullException.ThrowIfNull(entityBuilder);
            ArgumentNullException.ThrowIfNull(expression);

            _entityBuilder = entityBuilder;
            (_fullName, _propertyInfo) = GetPropertyInfo(expression);
            _alias = _fullName;
            UpdateConfigurations();
        }

        /// <summary>
        /// Enables filtering for the property.
        /// </summary>
        /// <returns>The current <see cref="MikesSifterPropertyBuilder"/> instance.</returns>
        public MikesSifterPropertyBuilder EnableFiltering()
        {
            _isFilterable = true;
            UpdateConfigurations();

            return this;
        }

        /// <summary>
        /// Enables sorting for the property.
        /// </summary>
        /// <returns>The current <see cref="MikesSifterPropertyBuilder"/> instance.</returns>
        public MikesSifterPropertyBuilder EnableSorting()
        {
            _isSortable = true;
            UpdateConfigurations();

            return this;
        }

        /// <summary>
        /// Sets an alias for the property.
        /// </summary>
        /// <param name="alias">The alias to set for the property.</param>
        /// <returns>The current <see cref="MikesSifterPropertyBuilder"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="alias"/> is null or whitespace.</exception>
        public MikesSifterPropertyBuilder HasAlias(string alias)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(alias);
    
            _alias = alias;
            UpdateConfigurations();

            return this;
        }

        /// <summary>
        /// Adds a custom filter for the property with the specified filtering operator.
        /// </summary>
        /// <param name="operator">The filtering operator to apply.</param>
        /// <param name="customFilter">The custom filter delegate to use for filtering.</param>
        /// <returns>The current <see cref="MikesSifterPropertyBuilder"/> instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="customFilter"/> is null.</exception>
        public MikesSifterPropertyBuilder HasCustomFilter(FilteringOperators @operator, CustomFilterDelegate<TEntity> customFilter)
        {
            ArgumentNullException.ThrowIfNull(customFilter);

            _customFilters[@operator] = ConvertToFunc(customFilter);
            UpdateConfigurations();

            return this;
        }
        
        private static (string fullName, PropertyInfo propertyInfo) GetPropertyInfo(Expression<Func<TEntity, object?>> expression)
        {
            if (expression.Body is not MemberExpression body)
            {
                var uBody = (UnaryExpression)expression.Body;
                body = (MemberExpression)uBody.Operand;
            }

            var propertyInfo = body.Member as PropertyInfo;
            ArgumentNullException.ThrowIfNull(propertyInfo, nameof(PropertyInfo));

            var stack = new Stack<string>();
            while (true)
            {
                stack.Push(body.Member.Name);
                if (body.Expression is not MemberExpression bodyExpression)
                {
                    break;
                }

                body = bodyExpression;
            }

            return (string.Join('.', stack.ToArray()), propertyInfo);
        }

        private void UpdateConfigurations()
        {
            var configuration = new MikesSifterPropertyConfiguration(_alias, _fullName, _customFilters, _isFilterable, _isSortable);
            _entityBuilder._configurations[_propertyInfo] = configuration;
        }
        
        private static Func<string?, Expression<Func<TEntity, bool>>> ConvertToFunc(CustomFilterDelegate<TEntity> customFilter)
        {
            return filterValue => customFilter(filterValue);
        }
    }

    internal override MikesSifterPropertyConfiguration FindConfiguration(string alias)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(alias);
        var result = _configurations.FirstOrDefault(e => e.Value.PropertyAlias.Equals(alias, StringComparison.Ordinal));
        return result.Value;
    }
}

public delegate Expression<Func<TEntity, bool>> CustomFilterDelegate<TEntity>(string? filterValue);

