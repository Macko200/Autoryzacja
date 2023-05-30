using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autoryzacja.Data.Migrations
{
    public partial class AktualizacjaUrlopy2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Urlopy",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Urlopy");
        }
    }
}
