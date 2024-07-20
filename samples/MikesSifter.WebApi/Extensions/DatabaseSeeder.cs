using Bogus;
using Bogus.Extensions.Canada;
using Bogus.Extensions.Denmark;
using Microsoft.EntityFrameworkCore;
using MikesSifter.WebApi.Models;

namespace MikesSifter.WebApi.Extensions;

public static class DatabaseSeeder
{
    private static readonly Faker<User> UserFaker = new Faker<User>()
        .RuleFor(e => e.Id, e => Guid.NewGuid())
        .RuleFor(e => e.FullName, e => e.Name.FullName())
        .RuleFor(e => e.Age, e => e.Random.Int(0, 100))
        .RuleFor(e => e.BirthDate, e => e.Person.DateOfBirth.ToUniversalTime());

    private const int UsersCount = 200;
    private static readonly (int from, int to) ProjectsRange = (5, 15);

    public static void Seed(this ModelBuilder modelBuilder)
    {
        var users = Enumerable.Range(0, UsersCount).Select(e => UserFaker.Generate()).ToList();
        var projects = new List<Project>();
        var passports = new List<Passport>();

        foreach (var user in users)
        {
            var faker = new Faker();
            var userProjects = Enumerable.Range(0, Random.Shared.Next(ProjectsRange.from, ProjectsRange.to)).Select(e => new Project()
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Title = faker.Commerce.ProductName(),
            }).ToList();

            var passport = new Passport
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Number = faker.Person.Sin(),
                Series = faker.Person.Cpr()
            };
            
            projects.AddRange(userProjects);
            passports.Add(passport);
        }

        modelBuilder.Entity<User>().HasData(users);
        modelBuilder.Entity<Project>().HasData(projects);
        modelBuilder.Entity<Passport>().HasData(passports);
    }
}