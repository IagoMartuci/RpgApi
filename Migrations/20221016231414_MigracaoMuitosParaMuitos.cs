using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RpgApi.Migrations
{
    public partial class MigracaoMuitosParaMuitos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Derrotas",
                table: "Personagens",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Disputas",
                table: "Personagens",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Vitorias",
                table: "Personagens",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Habilidades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dano = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Habilidades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonagemHabilidades",
                columns: table => new
                {
                    PersonagemId = table.Column<int>(type: "int", nullable: false),
                    HabilidadeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonagemHabilidades", x => new { x.PersonagemId, x.HabilidadeId });
                    table.ForeignKey(
                        name: "FK_PersonagemHabilidades_Habilidades_HabilidadeId",
                        column: x => x.HabilidadeId,
                        principalTable: "Habilidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonagemHabilidades_Personagens_PersonagemId",
                        column: x => x.PersonagemId,
                        principalTable: "Personagens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Habilidades",
                columns: new[] { "Id", "Dano", "Nome" },
                values: new object[,]
                {
                    { 1, 39, "Adormecer" },
                    { 2, 41, "Congelar" },
                    { 3, 37, "Hipnotizar" }
                });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 128, 255, 11, 172, 238, 7, 48, 156, 52, 52, 108, 48, 216, 216, 133, 38, 64, 93, 148, 205, 239, 16, 163, 211, 52, 108, 237, 194, 73, 110, 204, 70, 193, 46, 75, 60, 234, 108, 23, 30, 119, 48, 183, 231, 217, 145, 12, 83, 126, 40, 203, 120, 85, 40, 87, 125, 122, 122, 234, 123, 9, 220, 191, 180 }, new byte[] { 175, 130, 176, 225, 92, 139, 132, 183, 86, 215, 25, 214, 54, 12, 32, 72, 238, 154, 250, 111, 186, 184, 201, 210, 38, 158, 37, 76, 9, 97, 43, 74, 46, 234, 16, 253, 220, 1, 244, 249, 96, 103, 164, 83, 230, 22, 17, 53, 112, 87, 250, 195, 237, 233, 229, 105, 116, 62, 1, 179, 164, 235, 208, 184, 56, 120, 208, 15, 209, 189, 23, 34, 188, 190, 16, 146, 35, 18, 190, 102, 200, 122, 39, 241, 180, 127, 236, 54, 144, 253, 73, 59, 164, 181, 186, 168, 90, 173, 115, 59, 3, 228, 8, 254, 202, 42, 81, 253, 192, 19, 52, 239, 198, 81, 3, 164, 186, 196, 83, 136, 50, 232, 114, 125, 168, 7, 113, 28 } });

            migrationBuilder.InsertData(
                table: "PersonagemHabilidades",
                columns: new[] { "HabilidadeId", "PersonagemId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 2, 2 },
                    { 2, 3 },
                    { 3, 3 },
                    { 3, 4 },
                    { 1, 5 },
                    { 2, 6 },
                    { 3, 7 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonagemHabilidades_HabilidadeId",
                table: "PersonagemHabilidades",
                column: "HabilidadeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonagemHabilidades");

            migrationBuilder.DropTable(
                name: "Habilidades");

            migrationBuilder.DropColumn(
                name: "Derrotas",
                table: "Personagens");

            migrationBuilder.DropColumn(
                name: "Disputas",
                table: "Personagens");

            migrationBuilder.DropColumn(
                name: "Vitorias",
                table: "Personagens");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 110, 130, 232, 249, 71, 168, 17, 220, 200, 122, 37, 144, 48, 211, 241, 245, 166, 211, 13, 29, 56, 210, 94, 83, 14, 93, 222, 19, 132, 178, 153, 22, 222, 188, 161, 1, 22, 19, 60, 85, 81, 223, 148, 98, 93, 83, 212, 247, 190, 17, 55, 124, 111, 69, 3, 184, 196, 59, 196, 178, 213, 205, 97, 130 }, new byte[] { 155, 145, 156, 150, 217, 122, 156, 176, 120, 212, 65, 110, 245, 27, 118, 89, 141, 2, 201, 211, 229, 150, 215, 181, 143, 173, 38, 255, 123, 193, 61, 134, 179, 152, 142, 225, 207, 111, 179, 86, 237, 101, 80, 46, 47, 161, 80, 158, 169, 146, 161, 44, 81, 5, 201, 55, 153, 164, 238, 215, 174, 251, 40, 58, 113, 2, 72, 170, 94, 26, 28, 117, 114, 70, 109, 89, 9, 75, 190, 73, 9, 124, 16, 225, 45, 44, 35, 60, 88, 38, 60, 99, 115, 191, 218, 255, 145, 106, 237, 76, 84, 155, 16, 59, 238, 100, 205, 218, 101, 192, 249, 39, 219, 119, 4, 133, 55, 200, 83, 2, 174, 217, 33, 173, 155, 183, 232, 150 } });
        }
    }
}
