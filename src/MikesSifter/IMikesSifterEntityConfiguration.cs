namespace MikesSifter;

public interface IMikesSifterEntityConfiguration;

/// <summary>
/// Interface for configuring the sifter for a specific entity type.
/// </summary>
/// <typeparam name="TEntity">The type of the entity to configure.</typeparam>
public interface IMikesSifterEntityConfiguration<TEntity> : IMikesSifterEntityConfiguration
{
    /// <summary>
    /// Configures the sifter for the specified entity type.
    /// </summary>
    /// <param name="builder">The builder to use for configuration.</param>
    void Configure(MikesSifterEntityBuilder<TEntity> builder);
}