using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace versión_5_asp.Data.Migrations
{
    public partial class MigracionTrueques : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Trueques",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPerson = table.Column<int>(type: "int", nullable: false),
                    Proposition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Search = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Picture = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trueques", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Trueques");
        }
    }
}
