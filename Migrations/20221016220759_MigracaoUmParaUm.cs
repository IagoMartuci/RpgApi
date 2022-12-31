using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RpgApi.Migrations
{
    public partial class MigracaoUmParaUm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PersonagemId",
                table: "Armas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Armas",
                keyColumn: "Id",
                keyValue: 1,
                column: "PersonagemId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Armas",
                keyColumn: "Id",
                keyValue: 2,
                column: "PersonagemId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Armas",
                keyColumn: "Id",
                keyValue: 3,
                column: "PersonagemId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Armas",
                keyColumn: "Id",
                keyValue: 4,
                column: "PersonagemId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Armas",
                keyColumn: "Id",
                keyValue: 5,
                column: "PersonagemId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Armas",
                keyColumn: "Id",
                keyValue: 6,
                column: "PersonagemId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Armas",
                keyColumn: "Id",
                keyValue: 7,
                column: "PersonagemId",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 110, 130, 232, 249, 71, 168, 17, 220, 200, 122, 37, 144, 48, 211, 241, 245, 166, 211, 13, 29, 56, 210, 94, 83, 14, 93, 222, 19, 132, 178, 153, 22, 222, 188, 161, 1, 22, 19, 60, 85, 81, 223, 148, 98, 93, 83, 212, 247, 190, 17, 55, 124, 111, 69, 3, 184, 196, 59, 196, 178, 213, 205, 97, 130 }, new byte[] { 155, 145, 156, 150, 217, 122, 156, 176, 120, 212, 65, 110, 245, 27, 118, 89, 141, 2, 201, 211, 229, 150, 215, 181, 143, 173, 38, 255, 123, 193, 61, 134, 179, 152, 142, 225, 207, 111, 179, 86, 237, 101, 80, 46, 47, 161, 80, 158, 169, 146, 161, 44, 81, 5, 201, 55, 153, 164, 238, 215, 174, 251, 40, 58, 113, 2, 72, 170, 94, 26, 28, 117, 114, 70, 109, 89, 9, 75, 190, 73, 9, 124, 16, 225, 45, 44, 35, 60, 88, 38, 60, 99, 115, 191, 218, 255, 145, 106, 237, 76, 84, 155, 16, 59, 238, 100, 205, 218, 101, 192, 249, 39, 219, 119, 4, 133, 55, 200, 83, 2, 174, 217, 33, 173, 155, 183, 232, 150 } });

            migrationBuilder.CreateIndex(
                name: "IX_Armas_PersonagemId",
                table: "Armas",
                column: "PersonagemId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Armas_Personagens_PersonagemId",
                table: "Armas",
                column: "PersonagemId",
                principalTable: "Personagens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Armas_Personagens_PersonagemId",
                table: "Armas");

            migrationBuilder.DropIndex(
                name: "IX_Armas_PersonagemId",
                table: "Armas");

            migrationBuilder.DropColumn(
                name: "PersonagemId",
                table: "Armas");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 171, 211, 184, 116, 72, 47, 186, 233, 201, 64, 104, 110, 7, 35, 14, 125, 132, 94, 52, 214, 121, 96, 86, 234, 9, 223, 200, 57, 146, 23, 212, 195, 193, 8, 55, 238, 41, 200, 176, 67, 161, 173, 100, 218, 132, 82, 105, 31, 124, 239, 65, 232, 183, 214, 109, 27, 21, 83, 66, 115, 104, 183, 107, 112 }, new byte[] { 235, 245, 240, 185, 97, 248, 89, 213, 246, 81, 184, 159, 66, 232, 54, 10, 147, 65, 151, 97, 234, 179, 101, 230, 135, 105, 87, 64, 153, 14, 245, 22, 136, 190, 94, 216, 37, 194, 196, 85, 31, 53, 155, 80, 38, 40, 1, 37, 154, 183, 246, 1, 56, 26, 57, 13, 122, 91, 214, 53, 187, 165, 131, 68, 165, 199, 215, 39, 202, 198, 111, 18, 206, 247, 43, 54, 196, 198, 5, 56, 67, 54, 181, 106, 195, 199, 172, 163, 86, 222, 159, 175, 118, 217, 9, 113, 175, 1, 27, 169, 103, 129, 182, 4, 134, 177, 7, 48, 178, 11, 75, 84, 40, 107, 117, 59, 32, 155, 200, 87, 153, 144, 157, 255, 122, 174, 81, 89 } });
        }
    }
}
