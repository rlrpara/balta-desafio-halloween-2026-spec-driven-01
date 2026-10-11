using PasswordGenerator.Domain.Entities;

namespace PasswordGenerator.Service.Interfaces;

public interface IPasswordService
{
    Task<PasswordRecord> GenerateAsync(CancellationToken cancellationToken = default);

    Task<PasswordRecord?> GetAsync(Guid id, CancellationToken cancellationToken = default);
}
