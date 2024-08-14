namespace MikesSifter;

internal record MikesSifterEntityConfiguration(
    Type EntityType, 
    IReadOnlyCollection<MikesSifterPropertyConfiguration> PropertyConfigurations);