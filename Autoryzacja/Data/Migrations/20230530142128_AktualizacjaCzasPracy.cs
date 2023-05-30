using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autoryzacja.Data.Migrations
{
    public partial class AktualizacjaCzasPracy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CzasPracy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Rozpoczecie = table.Column<TimeSpan>(type: "time", nullable: false),
                    Zakonczenie = table.Column<TimeSpan>(type: "time", nullable: false),
                    PracaZdalna = table.Column<bool>(type: "bit", nullable: false),
                    IloscGodzin = table.Column<double>(type: "float", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CzasPracy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CzasPracy_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CzasPracy_UserId",
                table: "CzasPracy",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CzasPracy");
        }
    }
}
