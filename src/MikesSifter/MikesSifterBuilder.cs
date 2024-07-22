using System.Reflection;

namespace MikesSifter;

public class MikesSifterBuilder
{
    private readonly Dictionary<Type, MikesSifterEntityBuilder> _builders = new();

    /// <summary>
    /// Configures an entity of type using the specified configuration action.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity to configure.</typeparam>
    /// <param name="entityConfiguration">An action to configure the entity.</param>
    /// <returns>The current <see cref="MikesSifterBuilder"/> instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="entityConfiguration"/> is null.</exception>
    public MikesSifterBuilder Entity<TEntity>(Action<MikesSifterEntityBuilder<TEntity>> entityConfiguration) where TEntity : class
    {
        ArgumentNullException.ThrowIfNull(entityConfiguration);
    
        var result = new MikesSifterEntityBuilder<TEntity>();
        entityConfiguration.Invoke(result);
        _builders[typeof(TEntity)] = result;
        return this;
    }

    /// <summary>
    /// Applies the configuration from the specified configuration class <typeparamref name="TConfiguration"/>.
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration class to apply.</typeparam>
    /// <returns>The current <see cref="MikesSifterBuilder"/> instance.</returns>
    public MikesSifterBuilder ApplyConfiguration<TConfiguration>() where TConfiguration : class, IMikesSifterEntityConfiguration, new()
    {
        var configurationType = typeof(TConfiguration);
        var entityType = GetEntityTypeFromConfiguration();

        var builderType = typeof(MikesSifterEntityBuilder<>).MakeGenericType(entityType);
        var builder = Activator.CreateInstance(builderType);

        var configureMethod = configurationType.GetMethod(nameof(IMikesSifterEntityConfiguration<object>.Configure), new[] { builderType });
    
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

    /// <summary>
    /// Applies all entity configurations found in the specified assembly.
    /// </summary>
    /// <param name="assembly">The assembly to scan for configurations.</param>
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