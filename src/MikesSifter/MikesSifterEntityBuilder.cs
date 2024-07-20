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

        public MikesSifterPropertyBuilder EnableFiltering()
        {
            _isFilterable = true;
            UpdateConfigurations();

            return this;
        }

        public MikesSifterPropertyBuilder EnableSorting()
        {
            _isSortable = true;
            UpdateConfigurations();

            return this;
        }

        public MikesSifterPropertyBuilder HasAlias(string alias)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(alias);
            
            _alias = alias;
            UpdateConfigurations();

            return this;
        }

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

