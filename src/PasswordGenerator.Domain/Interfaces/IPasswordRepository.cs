using PasswordGenerator.Domain.Entities;

namespace PasswordGenerator.Domain.Interfaces;

public interface IPasswordRepository
{
    Task AddAsync(PasswordRecord password, CancellationToken cancellationToken = default);

    Task<PasswordRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
