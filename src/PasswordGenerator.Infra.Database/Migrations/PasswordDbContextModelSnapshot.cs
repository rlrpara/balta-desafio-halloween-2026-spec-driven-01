using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PasswordGenerator.Infra.Data.Context;

#nullable disable

namespace PasswordGenerator.Infra.Database.Migrations;

[DbContext(typeof(PasswordDbContext))]
partial class PasswordDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("ProductVersion", "10.0.12");
        modelBuilder.Entity("PasswordGenerator.Domain.Entities.PasswordRecord", b =>
        {
            b.Property<Guid>("Id").HasColumnType("TEXT");
            b.Property<string>("Password").IsRequired().HasMaxLength(256).HasColumnType("TEXT");
            b.HasKey("Id");
            b.ToTable("passwords");
        });
    }
}
