using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autoryzacja.Data.Migrations
{
    public partial class CreateUrlopyTable1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IloscDni",
                table: "Urlopy",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IloscDni",
                table: "Urlopy");
        }

    }
}
