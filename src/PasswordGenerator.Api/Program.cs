using PasswordGenerator.Domain.Interfaces;
using PasswordGenerator.Infra.Data.Repositories;
using PasswordGenerator.Infra.Database;
using PasswordGenerator.Service;
using PasswordGenerator.Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Passwords") ?? "Data Source=passwords.db";

builder.Services.AddProblemDetails();
builder.Services.AddPasswordDatabase(connectionString);
builder.Services.AddScoped<IPasswordRepository, PasswordRepository>();
builder.Services.AddScoped<IPasswordService, PasswordService>();

var app = builder.Build();
await app.Services.ApplyPasswordMigrationsAsync();

app.MapPost("/passwords", async (IPasswordService service, CancellationToken cancellationToken) =>
{
    var password = await service.GenerateAsync(cancellationToken);
    var response = new PasswordResponse(password.Id, password.Password);
    return Results.Created($"/passwords/{password.Id}", response);
});

app.MapGet("/passwords/{id}", async (string id, IPasswordService service, CancellationToken cancellationToken) =>
{
    if (!Guid.TryParse(id, out var passwordId))
    {
        return Results.Problem(
            statusCode: StatusCodes.Status400BadRequest,
            title: "Invalid password id",
            detail: "The id must be a valid GUID.");
    }

    var password = await service.GetAsync(passwordId, cancellationToken);
    return password is null
        ? Results.NotFound()
        : Results.Ok(new PasswordResponse(password.Id, password.Password));
});

app.Run();

public sealed record PasswordResponse(Guid Id, string Password);

public partial class Program;
