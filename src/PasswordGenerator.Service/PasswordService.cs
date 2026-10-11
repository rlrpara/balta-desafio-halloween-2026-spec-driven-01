using System.Security.Cryptography;
using PasswordGenerator.Domain.Entities;
using PasswordGenerator.Domain.Interfaces;
using PasswordGenerator.Service.Interfaces;

namespace PasswordGenerator.Service;

public sealed class PasswordService(IPasswordRepository repository) : IPasswordService
{
    private const int PasswordLength = 24;
    private const string Uppercase = "ABCDEFGHJKLMNPQRSTUVWXYZ";
    private const string Lowercase = "abcdefghijkmnopqrstuvwxyz";
    private const string Digits = "23456789";
    private const string SpecialCharacters = "!@#$%^&*()-_=+[]{}?";
    private static readonly string Alphabet = Uppercase + Lowercase + Digits + SpecialCharacters;

    public async Task<PasswordRecord> GenerateAsync(CancellationToken cancellationToken = default)
    {
        var characters = new char[PasswordLength];
        characters[0] = GetRandomCharacter(Uppercase);
        characters[1] = GetRandomCharacter(Lowercase);
        characters[2] = GetRandomCharacter(Digits);
        characters[3] = GetRandomCharacter(SpecialCharacters);

        for (var index = 4; index < characters.Length; index++)
        {
            characters[index] = GetRandomCharacter(Alphabet);
        }

        Shuffle(characters);

        var record = new PasswordRecord(Guid.NewGuid(), new string(characters));
        await repository.AddAsync(record, cancellationToken);
        return record;
    }

    public Task<PasswordRecord?> GetAsync(Guid id, CancellationToken cancellationToken = default) =>
        repository.GetByIdAsync(id, cancellationToken);

    private static char GetRandomCharacter(string source) => source[RandomNumberGenerator.GetInt32(source.Length)];

    private static void Shuffle(Span<char> characters)
    {
        for (var index = characters.Length - 1; index > 0; index--)
        {
            var otherIndex = RandomNumberGenerator.GetInt32(index + 1);
            (characters[index], characters[otherIndex]) = (characters[otherIndex], characters[index]);
        }
    }
}
