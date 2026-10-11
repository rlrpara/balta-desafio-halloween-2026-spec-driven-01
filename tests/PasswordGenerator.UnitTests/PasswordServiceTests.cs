using PasswordGenerator.Domain.Entities;
using PasswordGenerator.Domain.Interfaces;
using PasswordGenerator.Service;
using Xunit;

namespace PasswordGenerator.UnitTests;

public sealed class PasswordServiceTests
{
    [Fact]
    public async Task GenerateAsync_creates_password_with_required_rules()
    {
        var repository = new InMemoryPasswordRepository();
        var result = await new PasswordService(repository).GenerateAsync();

        Assert.True(result.Id != Guid.Empty);
        Assert.True(result.Password.Length >= 16);
        Assert.DoesNotContain(result.Password, char.IsWhiteSpace);
        Assert.Matches("[!@#$%^&*()\\-_=+\\[\\]{}?]", result.Password);
        Assert.Contains(repository.Items, item => item.Id == result.Id && item.Password == result.Password);
    }

    [Fact]
    public async Task GenerateAsync_creates_unique_guids_and_passwords()
    {
        var repository = new InMemoryPasswordRepository();
        var service = new PasswordService(repository);

        var first = await service.GenerateAsync();
        var second = await service.GenerateAsync();

        Assert.NotEqual(first.Id, second.Id);
        Assert.NotEqual(first.Password, second.Password);
    }

    [Fact]
    public async Task GetAsync_returns_existing_password()
    {
        var repository = new InMemoryPasswordRepository();
        var created = await new PasswordService(repository).GenerateAsync();

        var result = await new PasswordService(repository).GetAsync(created.Id);

        Assert.Same(created, result);
    }

    [Fact]
    public async Task GetAsync_returns_null_for_unknown_guid()
    {
        var repository = new InMemoryPasswordRepository();

        var result = await new PasswordService(repository).GetAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    private sealed class InMemoryPasswordRepository : IPasswordRepository
    {
        public List<PasswordRecord> Items { get; } = [];

        public Task AddAsync(PasswordRecord password, CancellationToken cancellationToken = default)
        {
            Items.Add(password);
            return Task.CompletedTask;
        }

        public Task<PasswordRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            Task.FromResult(Items.SingleOrDefault(item => item.Id == id));
    }
}
