using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using MikesSifter.Extensions;
using MikesSifter.WebApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));;
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

builder.Services.AddSifter<ApplicationSifter>();
builder.Services.AddDbContext<ApplicationDbContext>(e => e.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();