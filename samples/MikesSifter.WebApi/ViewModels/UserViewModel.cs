namespace MikesSifter.WebApi.ViewModels;

public record UserViewModel
{
    public Guid Id { get; set; }
    public DateTime BirthDate { get; set; }
    public required string FullName { get; set; }
    
    public bool Gender { get; set; }
    public required PassportViewModel Passport { get; set; }
    public required List<ProjectViewModel> Projects { get; set; }
}