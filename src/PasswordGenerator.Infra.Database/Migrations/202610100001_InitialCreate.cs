using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace PasswordGenerator.Infra.Database.Migrations;

[DbContext(typeof(PasswordGenerator.Infra.Data.Context.PasswordDbContext))]
[Migration("202610100001_InitialCreate")]
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "passwords",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Password = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_passwords", x => x.Id));
    }

    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropTable("passwords");
}
