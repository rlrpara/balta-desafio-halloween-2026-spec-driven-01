using Microsoft.EntityFrameworkCore;
using PasswordGenerator.Domain.Entities;
using PasswordGenerator.Domain.Interfaces;
using PasswordGenerator.Infra.Data.Context;

namespace PasswordGenerator.Infra.Data.Repositories;

public sealed class PasswordRepository(PasswordDbContext dbContext) : IPasswordRepository
{
    public async Task AddAsync(PasswordRecord password, CancellationToken cancellationToken = default)
    {
        dbContext.Passwords.Add(password);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<PasswordRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        dbContext.Passwords.AsNoTracking().SingleOrDefaultAsync(password => password.Id == id, cancellationToken);
}
