namespace MikesSifter.WebApi.Models;

public class Project
{
    public required Guid Id { get; init; }
    
    public required Guid UserId { get; init; }
    
    public required string Title { get; init; }
    
    public User User { get; init; } = null!;
}