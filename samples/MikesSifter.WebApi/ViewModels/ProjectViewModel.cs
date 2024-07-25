namespace MikesSifter.WebApi.ViewModels;

public record ProjectViewModel
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
}