using Microsoft.EntityFrameworkCore.Migrations;

namespace versión_5_asp.Data.Migrations
{
    public partial class MigracionEnlace : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "enlace",
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
                    table.PrimaryKey("PK_enlace", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "enlace");
        }
    }
}
