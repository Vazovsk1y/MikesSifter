# MikesSifter

MikesSifter is a versatile and extensible library designed to provide powerful filtering, sorting and paging capabilities in .NET applications.

## Installation

Minimum Requirements: .NET 8.0.x

[Download from NuGet](https://www.nuget.org/packages/MikesSifter/)

##### Package Manager

```powershell
NuGet\Install-Package MikesSifter -Version *version_number*
```

##### .NET CLI

```cmd
dotnet add package MikesSifter --version *version_number*
```

## Usage for ASP.NET Core WebApi

In this example, consider an app with a `User` entity that can have many projects. We'll use MikesSifter to add sorting, filtering and pagination capabilities when retrieving all available users.

### 1. Define the application sifter.

Inherit the base implementation `MikesSifter` and override the `Configure` method.

```csharp
public class ApplicationSifter : MikesSifter
{
    protected override void Configure(MikesSifterBuilder builder)
    {
        // Here apply your configurations.
    }
}
```

### 2. Configure the properties you want to sort/filter in your models.

#### 2.1. Using a separate entity configuration class.

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
            .Property(e => e.Gender)
            .EnableSorting()
            .EnableFiltering();

        builder
            .Property(e => e.BirthDate)
            .EnableFiltering()
            .EnableSorting();

        builder
            .Property(e => e.Projects)
            .EnableFiltering()
            .HasCustomFilters(e =>
            {
                e.WithFilter(FilteringOperators.Contains, filterValue =>
                {
                    ArgumentException.ThrowIfNullOrWhiteSpace(filterValue);
                    return u => u.Projects.Any(o => o.Id == Guid.Parse(filterValue));
                });
            });

        builder
            .Property(e => e.Passport.Number)
            .EnableFiltering()
            .EnableSorting()
            .HasAlias("user_passportNumber");
    }
}
```

Apply particular configuration by calling `ApplyConfiguration<T>`:

```csharp

protected override void Configure(MikesSifterBuilder builder)
{
     builder.ApplyConfiguration<UserSifterConfiguration>();
}

```

Apply configurations from a particular assembly by calling `ApplyConfigurationsFromAssembly`:

```csharp
protected override void Configure(MikesSifterBuilder builder)
{
     builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
}
```

#### 2.2. Using Fluent API without defining a separate configuration class.

```csharp

protected override void Configure(MikesSifterBuilder builder)
{
     builder.Entity<User>(e =>
     {
          e.Property(i => i.FullName)
              .EnableFiltering()
              .EnableSorting();

          e.Property(i => i.Gender)
              .EnableSorting()
              .EnableFiltering();

          e.Property(i => i.BirthDate)
              .EnableFiltering()
              .EnableSorting();

          e.Property(e => e.Projects)
              .EnableFiltering()
              .HasCustomFilters(e =>
              {
                  e.WithFilter(FilteringOperators.Contains, filterValue =>
                  {
                      ArgumentException.ThrowIfNullOrWhiteSpace(filterValue);
                      return u => u.Projects.Any(o => o.Id == Guid.Parse(filterValue));
                  });
              });

          e.Property(i => i.Passport.Number)
              .EnableFiltering()
              .EnableSorting()
              .HasAlias("user_passportNumber");
     });
}

```

### 3. Implement `IMikesSifterModel`.

In our example, we will use a custom model implementation as the POST body. However, you can implement your own using, for example, GET method with query parameters.

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

### 4. Dependency Injection (DI).

Add the application sifter to the services by calling the `AddSifter` extension method.

```csharp
builder.Services.AddSifter<ApplicationSifter>();
```

### 5. Let's use.

Inject `IMikesSifter` into controller to use the sifter capabilities.

#### 5.1. Apply filtering, sorting and paging all together by calling the `Apply` method.

```csharp
[HttpPost("full")]
public IActionResult Full(ApplicationSifterModel model)
{
    var result = sifter.Apply(dbContext
        .Users
        .Include(e => e.Projects)
        .Include(e => e.Passport), model);
    
    return Ok(result.Select(e => e.ToViewModel()).ToList());
}
```

#### 5.2. Apply only filtering by calling the `ApplyFiltering` method.

```csharp
[HttpPost("filtering")]
public IActionResult OnlyFiltering(FilteringOptions filteringOptions)
{
    var result = sifter.ApplyFiltering(dbContext
        .Users
        .Include(e => e.Projects)
        .Include(e => e.Passport), filteringOptions);
    
    return Ok(result.Select(e => e.ToViewModel()).ToList());
}
```

#### 5.3. Apply only sorting by calling the `ApplySorting` method.

```csharp
[HttpPost("sorting")]
public IActionResult OnlySorting(SortingOptions sortingOptions)
{
    var result = sifter.ApplySorting(dbContext
        .Users
        .Include(e => e.Projects)
        .Include(e => e.Passport), sortingOptions);
    
    return Ok(result.Select(e => e.ToViewModel()).ToList());
}
```

#### 5.4. Apply only paging by calling the `ApplyPaging` method.

```csharp
[HttpPost("paging")]
public IActionResult OnlyPaging(PagingOptions pagingOptions)
{
    var result = sifter.ApplyPaging(dbContext
        .Users
        .Include(e => e.Projects)
        .Include(e => e.Passport), pagingOptions);
    
    return Ok(result.Select(e => e.ToViewModel()).ToList());
}
```