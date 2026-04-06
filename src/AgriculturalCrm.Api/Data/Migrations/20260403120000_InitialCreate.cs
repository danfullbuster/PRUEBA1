using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgriculturalCrm.Api.Data.Migrations;

public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Clientes",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                Email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false),
                Telefono = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                NombreFinca = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                Hectareas = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                CreadoEn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Clientes", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Clientes_Email",
            table: "Clientes",
            column: "Email",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Clientes");
    }
}
