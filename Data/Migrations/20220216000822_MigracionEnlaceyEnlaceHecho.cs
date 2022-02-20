using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace versión_5_asp.Data.Migrations
{
    public partial class MigracionEnlaceyEnlaceHecho : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Enlace",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdMi = table.Column<int>(type: "int", nullable: false),
                    IdSu = table.Column<int>(type: "int", nullable: false),
                    IdPersonaMi = table.Column<int>(type: "int", nullable: false),
                    IdPersonaSu = table.Column<int>(type: "int", nullable: false),
                    Si = table.Column<bool>(type: "bit", nullable: false),
                    No = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enlace", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnlaceHecho",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdMi = table.Column<int>(type: "int", nullable: false),
                    IdSu = table.Column<int>(type: "int", nullable: false),
                    IdPersonaMi = table.Column<int>(type: "int", nullable: false),
                    IdPersonaSu = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnlaceHecho", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Enlace");

            migrationBuilder.DropTable(
                name: "EnlaceHecho");
        }
    }
}
