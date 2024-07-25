using Microsoft.EntityFrameworkCore;
using MikesSifter.WebApi.Extensions;
using MikesSifter.WebApi.Models;

namespace MikesSifter.WebApi.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; init; }

    public DbSet<Project> Projects { get; init; }

    public DbSet<Passport> Passports { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder
            .Entity<User>(e =>
            {
                e.HasKey(i => i.Id);
                e.HasMany(c => c.Projects).WithOne(a => a.User).HasForeignKey(d => d.UserId);
                e.HasOne(p => p.Passport).WithOne(i => i.User).HasForeignKey<Passport>(p => p.UserId);
            });

        modelBuilder
            .Entity<Project>(e =>
            {
                e.HasKey(i => i.Id);
            });

        modelBuilder
            .Entity<Passport>(e =>
            {
                e.HasKey(i => i.Id);
            });

        modelBuilder.Seed();
    }
}