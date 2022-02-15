using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace versión_5_asp.Data.Migrations
{
    public partial class MigracionEnlacesHechos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "enlaceHecho",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdMi = table.Column<int>(type: "int", nullable: false),
                    IdSu = table.Column<int>(type: "int", nullable: false),
                    IdPersonaMi = table.Column<int>(type: "int", nullable: false),
                    IdPersonaSu = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "DateTime", nullable: false)
                   
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_enlaceHecho", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "enlaceHecho");
        }
    }
}
