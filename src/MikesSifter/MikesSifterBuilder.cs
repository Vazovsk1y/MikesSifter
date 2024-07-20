using System.Reflection;

namespace MikesSifter;

public class MikesSifterBuilder
{
    private readonly Dictionary<Type, MikesSifterEntityBuilder> _builders = new();

    public MikesSifterEntityBuilder<TEntity> Entity<TEntity>() where TEntity : class
    {
        var result = new MikesSifterEntityBuilder<TEntity>();
        _builders[typeof(TEntity)] = result;
        return result;
    }

    public MikesSifterBuilder ApplyConfiguration<TConfiguration>() where TConfiguration : class, IMikesSifterEntityConfiguration, new()
    {
        var configurationType = typeof(TConfiguration);
        var entityType = GetEntityTypeFromConfiguration();

        var builderType = typeof(MikesSifterEntityBuilder<>).MakeGenericType(entityType);
        var builder = Activator.CreateInstance(builderType);

        var configureMethod = configurationType.GetMethod(nameof(IMikesSifterEntityConfiguration<object>.Configure), [ builderType ]);
        
        var configuration = new TConfiguration();
        configureMethod!.Invoke(configuration, [ builder ]);

        var castedBuilder = builder as MikesSifterEntityBuilder;
        ArgumentNullException.ThrowIfNull(castedBuilder);
        
        _builders[entityType] = castedBuilder;
        return this;

        Type GetEntityTypeFromConfiguration()
        {
            var interfaceType = configurationType
                .GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMikesSifterEntityConfiguration<>));

            return interfaceType.GetGenericArguments()[0];
        }
    }
    
    public void ApplyConfigurationsFromAssembly(Assembly assembly)
    {
        var configurationTypes = assembly
            .GetTypes()
            .Where(type => type.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMikesSifterEntityConfiguration<>)));

        foreach (var configType in configurationTypes)
        {
            var applyConfigurationMethod = typeof(MikesSifterBuilder).GetMethod(nameof(ApplyConfiguration))!.MakeGenericMethod(configType);
            applyConfigurationMethod.Invoke(this, null);
        }
    }
    
    internal MikesSifterEntityBuilder? FindBuilder(Type entityType)
    {
        _ = _builders.TryGetValue(entityType, out var builder);
        return builder;
    }
}