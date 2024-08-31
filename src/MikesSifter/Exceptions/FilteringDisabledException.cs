namespace MikesSifter.Exceptions;

public class FilteringDisabledException : MikesSifterException
{
    public Type EntityType { get; }
    
    public string PropertyAlias { get; }

    internal FilteringDisabledException(Type entityType, string propertyAlias) 
        : base($"Filtering is disabled for the property [ \"{propertyAlias}\" ] on entity type [ \"{entityType.FullName}\" ]. Call '{nameof(MikesSifterPropertyBuilder<Type>.EnableFiltering)}'.")
    {
        EntityType = entityType;
        PropertyAlias = propertyAlias;
    }
}