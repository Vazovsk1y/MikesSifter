namespace MikesSifter.WebApi.Models;

public record Passport
{
    public required Guid Id { get; init; }
    
    public required Guid UserId { get; init; }
    
    public required string Number { get; init; }
    
    public required string Series { get; init; }

    public User User { get; init; } = null!;
}