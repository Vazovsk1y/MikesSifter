using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MikesSifter.Filtering;
using MikesSifter.Paging;
using MikesSifter.Sorting;
using MikesSifter.WebApi.Extensions;
using MikesSifter.WebApi.Infrastructure;

namespace MikesSifter.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(
    IMikesSifter sifter,
    ApplicationDbContext dbContext) : ControllerBase
{
    [HttpPost("full")]
    public IActionResult Page(ApplicationSifterModel model)
    {
        var result = sifter.Apply(dbContext.Users.Include(e => e.Projects).Include(e => e.Passport), model);
        return Ok(result.Select(e => e.ToViewModel()).ToList());
    }

    [HttpPost("filtering")]
    public IActionResult Filtering(FilteringOptions filteringOptions)
    {
        var result = sifter.ApplyFiltering(dbContext.Users.Include(e => e.Projects).Include(e => e.Passport), filteringOptions);
        return Ok(result.Select(e => e.ToViewModel()).ToList());
    }

    [HttpPost("sorting")]
    public IActionResult Sorting(SortingOptions sortingOptions)
    {
        var result = sifter.ApplySorting(dbContext.Users.Include(e => e.Projects).Include(e => e.Passport), sortingOptions);
        return Ok(result.Select(e => e.ToViewModel()).ToList());
    }

    [HttpPost("paging")]
    public IActionResult Paging(PagingOptions pagingOptions)
    {
        var result = sifter.ApplyPaging(dbContext.Users.Include(e => e.Projects).Include(e => e.Passport), pagingOptions);
        return Ok(result.Select(e => e.ToViewModel()).ToList());
    }
}