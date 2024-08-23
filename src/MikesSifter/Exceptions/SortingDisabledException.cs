namespace MikesSifter.Exceptions;

public class SortingDisabledException : MikesSifterException
{
    public Type EntityType { get; }
    
    public string PropertyAlias { get; }

    internal SortingDisabledException(Type entityType, string propertyAlias) : 
        base($"Sorting is disabled for the property [ \"{propertyAlias}\" ] on entity type [ \"{entityType.FullName}\" ]. Call '{nameof(MikesSifterPropertyBuilder<Type>.EnableSorting)}'.")
    {
        EntityType = entityType;
        PropertyAlias = propertyAlias;
    }
}