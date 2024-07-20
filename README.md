# MikesSifter

MikesSifter is a versatile and extensible library designed to provide powerful filtering, sorting, and paging capabilities for your IQueryable data sources in .NET.

## Installation

Minimum Requirements: .NET 8.0.x

[Download from NuGet](https://www.nuget.org/packages/MikesSifter/)

##### PowerShell

```powershell
NuGet\Install-Package MikesSifter -Version *version_number*
```

##### Cmd

```cmd
dotnet add package MikesSifter --version *version_number*
```

## Usage

In this example, consider an app with a `User` entity that can have many projects. We'll use MikesSifter to add sorting, filtering, and pagination capabilities when retrieving all available users.

### 1. Configure the properties you want to sort/filter in your models.

#### 1.1. Using a modular entity configuration class.

```csharp
public class UserSifterConfiguration : IMikesSifterEntityConfiguration<User>
{
    public void Configure(MikesSifterEntityBuilder<User> builder)
    {
        builder
            .Property(e => e.FullName)
            .EnableFiltering()
            .EnableSorting();

        builder
            .Property(e => e.Age)
            .EnableSorting()
            .EnableFiltering();

        builder
            .Property(e => e.BirthDate)
            .EnableFiltering()
            .EnableSorting();

        builder
            .Property(e => e.Projects)
            .EnableFiltering()
            .HasCustomFilter(FilteringOperators.Contains, filterValue =>
            {
                ArgumentException.ThrowIfNullOrWhiteSpace(filterValue);
                return u => u.Projects.Any(e => e.Id == Guid.Parse(filterValue));
            });

        builder
            .Property(e => e.Passport.Number)
            .EnableFiltering()
            .EnableSorting()
            .HasAlias("user_passportNumber");
    }
}
```

Apply configurations by calling `ApplyConfiguration<T>`:

```csharp
builder.ApplyConfiguration<UserSifterConfiguration>();
```

Apply configurations from a particular assembly by calling `ApplyConfigurationsFromAssembly`:

```csharp
builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
```

#### 1.2. Using Fluent API without defining a separate configuration class.

```csharp
builder.Entity<User>(e =>
{
    e.Property(i => i.FullName)
        .EnableFiltering()
        .EnableSorting();

    e.Property(i => i.Age)
        .EnableSorting()
        .EnableFiltering();

    e.Property(i => i.BirthDate)
        .EnableFiltering()
        .EnableSorting();

    e.Property(i => i.Projects)
        .EnableFiltering()
        .HasCustomFilter(FilteringOperators.Contains, filterValue =>
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(filterValue);
            return u => u.Projects.Any(pr => pr.Id == Guid.Parse(filterValue));
        });

    e.Property(i => i.Passport.Number)
        .EnableFiltering()
        .EnableSorting()
        .HasAlias("user_passportNumber");
});
```

### 2. Implement `IMikesSifterModel`.

In our example, we will use a custom model implementation as the POST body. However, you can implement your own using, for example, GET query parameters.

```csharp
public sealed class ApplicationSifterModel : IMikesSifterModel
{
    public FilteringOptions? FilteringOptions { get; init; }
    
    public SortingOptions? SortingOptions { get; init; }
    
    public PagingOptions? PagingOptions { get; init; }

    public FilteringOptions? GetFilteringOptions() => FilteringOptions;

    public SortingOptions? GetSortingOptions() => SortingOptions;

    public PagingOptions? GetPagingOptions() => PagingOptions;
}
```

### 3. Define the application sifter.

Inherit the base implementation `MikesSifter` and override the `Configure` method.

```csharp
public class ApplicationSifter : MikesSifter
{
    protected override void Configure(MikesSifterBuilder builder)
    {
        builder.ApplyConfiguration<UserSifterConfiguration>();
    }
}
```

### 4. Dependency Injection (DI).

Add the application sifter to the services by calling the `AddSifter` extension method.

```csharp
builder.Services.AddSifter<ApplicationSifter>();
```

### 5. Let's use.

Inject `IMikesSifter` whenever you want to use the sifter capabilities.

#### 5.1. Apply filtering/sorting/paging by calling the `Apply` method.

```csharp
[HttpPost("full")]
public IActionResult Page(ApplicationSifterModel model)
{
    var result = sifter.Apply(dbContext.Users.Include(e => e.Projects).Include(e => e.Passport), model);
    return Ok(result.Select(e => e.ToViewModel()).ToList());
}
```

#### 5.2. Apply only filtering by calling the `ApplyFiltering` method.

```csharp
[HttpPost("filtering")]
public IActionResult Filtering(FilteringOptions filteringOptions)
{
    var result = sifter.ApplyFiltering(dbContext.Users.Include(e => e.Projects).Include(e => e.Passport), filteringOptions);
    return Ok(result.Select(e => e.ToViewModel()).ToList());
}
```

#### 5.3. Apply only sorting by calling the `ApplySorting` method.

```csharp
[HttpPost("sorting")]
public IActionResult Sorting(SortingOptions sortingOptions)
{
    var result = sifter.ApplySorting(dbContext.Users.Include(e => e.Projects).Include(e => e.Passport), sortingOptions);
    return Ok(result.Select(e => e.ToViewModel()).ToList());
}
```

#### 5.4. Apply only paging by calling the `ApplyPaging` method.

```csharp
[HttpPost("paging")]
public IActionResult Paging(PagingOptions pagingOptions)
{
    var result = sifter.ApplyPaging(dbContext.Users.Include(e => e.Projects).Include(e => e.Passport), pagingOptions);
    return Ok(result.Select(e => e.ToViewModel()).ToList());
}
```