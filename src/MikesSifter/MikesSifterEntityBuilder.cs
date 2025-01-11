using System.Linq.Expressions;
using System.Reflection;

namespace MikesSifter;

public abstract class MikesSifterEntityBuilder
{
    internal abstract EntityConfiguration Build();
}

public sealed class MikesSifterEntityBuilder<TEntity> : MikesSifterEntityBuilder
{
    private readonly Dictionary<PropertyInfo, MikesSifterPropertyBuilder<TEntity>> _builders = new();
    
    /// <summary>
    /// Configures a property of the entity type using the specified property expression.
    /// </summary>
    /// <param name="expression">An expression that identifies the property to configure.</param>
    /// <returns>A <see cref="MikesSifterPropertyBuilder{TEntity}"/> to further configure the property.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="expression"/> is null.</exception>
    public MikesSifterPropertyBuilder<TEntity> Property(Expression<Func<TEntity, object?>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);
        var (targetPropertyPath, propertyInfo) = GetPropertyDescription(expression);
        
        var builder = new MikesSifterPropertyBuilder<TEntity>(propertyInfo, targetPropertyPath);
        _builders[propertyInfo] = builder;
        return builder;
    }
    
    internal override EntityConfiguration Build()
    {
        return new EntityConfiguration(typeof(TEntity), _builders.Select(e => e.Value.Build()).ToList());
    }
    
    private static (string targetPropertyPath, PropertyInfo propertyInfo) GetPropertyDescription(Expression<Func<TEntity, object?>> expression)
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
}