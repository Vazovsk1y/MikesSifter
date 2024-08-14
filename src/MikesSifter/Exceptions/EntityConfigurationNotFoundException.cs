namespace MikesSifter.Exceptions;

public class EntityConfigurationNotFoundException : MikesSifterException
{
    public Type EntityType { get; }
    
    internal EntityConfigurationNotFoundException(Type entityType) : base($"No entity configuration found for type [ \"{entityType.FullName}\" ].")
    {
        EntityType = entityType;
    }
}