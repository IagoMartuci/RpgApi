using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RpgApi.Migrations
{
    public partial class MigrationArmas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Armas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dano = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Armas", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Armas",
                columns: new[] { "Id", "Dano", "Nome" },
                values: new object[,]
                {
                    { 1, 25, "Espada de Duas Mãos" },
                    { 2, 25, "Machado de Duas Mãos" },
                    { 3, 25, "Clava de Duas Mãos" },
                    { 4, 20, "Espada e Escudo" },
                    { 5, 20, "Machado e Escudo" },
                    { 6, 20, "Clava e Escudo" },
                    { 7, 15, "Cajado Elemental" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Armas");
        }
    }
}
