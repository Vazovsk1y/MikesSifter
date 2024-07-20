namespace MikesSifter;

public interface IMikesSifterEntityConfiguration;
public interface IMikesSifterEntityConfiguration<T> : IMikesSifterEntityConfiguration
{
    void Configure(MikesSifterEntityBuilder<T> builder);
}