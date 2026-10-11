using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PasswordGenerator.Infra.Data.Context;

namespace PasswordGenerator.Infra.Database;

public static class DatabaseConfiguration
{
    public static IServiceCollection AddPasswordDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<PasswordDbContext>(options => options.UseSqlite(
            connectionString,
            sqlite => sqlite.MigrationsAssembly(typeof(DatabaseConfiguration).Assembly.GetName().Name)));

        return services;
    }

    public static async Task ApplyPasswordMigrationsAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        await using var scope = services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PasswordDbContext>();
        await dbContext.Database.MigrateAsync(cancellationToken);
    }
}
