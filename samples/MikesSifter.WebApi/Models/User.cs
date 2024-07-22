namespace MikesSifter.WebApi.Models;

public class User
{
    public required Guid Id { get; init; }
    
    public required string FullName { get; init; }

    public required bool Gender { get; init; }
    
    public required DateTime BirthDate { get; init; }

    public Passport Passport { get; init; } = null!;
    
    public IEnumerable<Project> Projects { get; init; } = new List<Project>();
}