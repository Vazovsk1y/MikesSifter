using MikesSifter.WebApi.Models;
using MikesSifter.WebApi.ViewModels;

namespace MikesSifter.WebApi.Extensions;

public static class Mapper
{
    public static UserViewModel ToViewModel(this User user)
    {
        return new UserViewModel
        {
            Id = user.Id,
            BirthDate = user.BirthDate,
            FullName = user.FullName,
            Age = user.Age,
            Passport = new PassportViewModel
            {
                Series = user.Passport.Series,
                Number = user.Passport.Number
            },
            Projects = user.Projects.Select(p => new ProjectViewModel
            {
                Id = p.Id,
                Title = p.Title
            }).ToList()
        };
    }
}