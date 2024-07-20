namespace MikesSifter.Exceptions;

public class EntityBuilderNotFoundException : MikesSifterException
{
    public Type EntityType { get; }
    
    internal EntityBuilderNotFoundException(Type entityType) : base($"No entity builder found for type [ \"{entityType.FullName}\" ].")
    {
        EntityType = entityType;
    }
}