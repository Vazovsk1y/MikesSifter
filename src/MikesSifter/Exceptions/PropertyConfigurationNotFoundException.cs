namespace MikesSifter.Exceptions;

public class PropertyConfigurationNotFoundException : MikesSifterException
{
    public Type EntityType { get; }
    
    public string PropertyAlias { get; }
    
    internal PropertyConfigurationNotFoundException(Type entityType, string propertyAlias) : base($"Property configuration not found for property [ \"{propertyAlias}\" ] on entity type [ \"{entityType.FullName}\" ].")
    {
        EntityType = entityType;
        PropertyAlias = propertyAlias;
    }
}