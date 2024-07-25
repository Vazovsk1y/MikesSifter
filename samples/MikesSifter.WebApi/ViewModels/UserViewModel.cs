namespace MikesSifter.WebApi.ViewModels;

public record UserViewModel
{
    public  required Guid Id { get; init; }
    public required DateTime BirthDate { get; init; }
    public required string FullName { get; init; }
    public required bool Gender { get; init; }
    public required PassportViewModel Passport { get; init; }
    public required List<ProjectViewModel> Projects { get; init; }
}