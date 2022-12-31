using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RpgApi.Migrations
{
    public partial class MigracaoUsuario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Personagens",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<byte[]>(
                name: "FotoPersonagem",
                table: "Personagens",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Personagens",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Armas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Foto = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    DataAcesso = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Perfil = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "Jogador"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "DataAcesso", "Email", "Foto", "Latitude", "Longitude", "PasswordHash", "PasswordSalt", "Perfil", "UserName" },
                values: new object[] { 1, null, null, null, null, null, new byte[] { 171, 211, 184, 116, 72, 47, 186, 233, 201, 64, 104, 110, 7, 35, 14, 125, 132, 94, 52, 214, 121, 96, 86, 234, 9, 223, 200, 57, 146, 23, 212, 195, 193, 8, 55, 238, 41, 200, 176, 67, 161, 173, 100, 218, 132, 82, 105, 31, 124, 239, 65, 232, 183, 214, 109, 27, 21, 83, 66, 115, 104, 183, 107, 112 }, new byte[] { 235, 245, 240, 185, 97, 248, 89, 213, 246, 81, 184, 159, 66, 232, 54, 10, 147, 65, 151, 97, 234, 179, 101, 230, 135, 105, 87, 64, 153, 14, 245, 22, 136, 190, 94, 216, 37, 194, 196, 85, 31, 53, 155, 80, 38, 40, 1, 37, 154, 183, 246, 1, 56, 26, 57, 13, 122, 91, 214, 53, 187, 165, 131, 68, 165, 199, 215, 39, 202, 198, 111, 18, 206, 247, 43, 54, 196, 198, 5, 56, 67, 54, 181, 106, 195, 199, 172, 163, 86, 222, 159, 175, 118, 217, 9, 113, 175, 1, 27, 169, 103, 129, 182, 4, 134, 177, 7, 48, 178, 11, 75, 84, 40, 107, 117, 59, 32, 155, 200, 87, 153, 144, 157, 255, 122, 174, 81, 89 }, "Admin", "UsuarioAdmin" });

            migrationBuilder.CreateIndex(
                name: "IX_Personagens_UsuarioId",
                table: "Personagens",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Personagens_Usuarios_UsuarioId",
                table: "Personagens",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Personagens_Usuarios_UsuarioId",
                table: "Personagens");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Personagens_UsuarioId",
                table: "Personagens");

            migrationBuilder.DropColumn(
                name: "FotoPersonagem",
                table: "Personagens");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Personagens");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Personagens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Armas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
