using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PasswordGenerator.Infra.Data.Context;
using Xunit;

namespace PasswordGenerator.IntegrationTests;

public sealed class PasswordApiTests : IClassFixture<PasswordApiFactory>
{
    private readonly HttpClient client;

    public PasswordApiTests(PasswordApiFactory factory) => client = factory.CreateClient();

    [Fact]
    public async Task Post_then_get_returns_persisted_password()
    {
        var create = await client.PostAsync("/passwords", content: null);
        var created = await create.Content.ReadFromJsonAsync<PasswordResponse>();

        Assert.Equal(HttpStatusCode.Created, create.StatusCode);
        Assert.NotNull(created);

        var get = await client.GetAsync($"/passwords/{created!.Id}");
        var found = await get.Content.ReadFromJsonAsync<PasswordResponse>();

        Assert.Equal(HttpStatusCode.OK, get.StatusCode);
        Assert.Equal(created, found);
    }

    [Fact]
    public async Task Invalid_guid_returns_problem_details_and_bad_request()
    {
        var response = await client.GetAsync("/passwords/not-a-guid");
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        Assert.NotNull(problem);
        Assert.Equal(StatusCodes.Status400BadRequest, problem!.Status);
        Assert.Equal("Invalid password id", problem.Title);
        Assert.Equal("The id must be a valid GUID.", problem.Detail);
    }

    [Fact]
    public async Task Unknown_guid_returns_not_found()
    {
        var response = await client.GetAsync($"/passwords/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    private sealed record PasswordResponse(Guid Id, string Password);
}

public sealed class PasswordApiFactory : WebApplicationFactory<Program>
{
    private readonly SqliteConnection connection = new("Data Source=:memory:");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        connection.Open();
        builder.ConfigureServices(services =>
        {
            var descriptor = services.Single(item => item.ServiceType == typeof(DbContextOptions<PasswordDbContext>));
            services.Remove(descriptor);
            services.AddDbContext<PasswordDbContext>(options => options.UseSqlite(connection,
                sqlite => sqlite.MigrationsAssembly("PasswordGenerator.Infra.Database")));
        });
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            connection.Dispose();
        }

        base.Dispose(disposing);
    }
}
