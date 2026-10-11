namespace PasswordGenerator.Domain.Entities;

public sealed class PasswordRecord
{
    private PasswordRecord()
    {
    }

    public PasswordRecord(Guid id, string password)
    {
        Id = id;
        Password = password;
    }

    public Guid Id { get; private set; }

    public string Password { get; private set; } = string.Empty;
}
