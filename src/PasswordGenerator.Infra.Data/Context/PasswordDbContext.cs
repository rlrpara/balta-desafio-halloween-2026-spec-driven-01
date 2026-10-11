using Microsoft.EntityFrameworkCore;
using PasswordGenerator.Domain.Entities;

namespace PasswordGenerator.Infra.Data.Context;

public sealed class PasswordDbContext(DbContextOptions<PasswordDbContext> options) : DbContext(options)
{
    public DbSet<PasswordRecord> Passwords => Set<PasswordRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PasswordRecord>(entity =>
        {
            entity.ToTable("passwords");
            entity.HasKey(password => password.Id);
            entity.Property(password => password.Id).ValueGeneratedNever();
            entity.Property(password => password.Password).HasMaxLength(256).IsRequired();
        });
    }
}
