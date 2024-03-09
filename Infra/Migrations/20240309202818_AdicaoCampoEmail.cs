using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class AdicaoCampoEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AlunoIdId",
                table: "Users",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Alunos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AlunoIdId",
                table: "Users",
                column: "AlunoIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Alunos_AlunoIdId",
                table: "Users",
                column: "AlunoIdId",
                principalTable: "Alunos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Alunos_AlunoIdId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AlunoIdId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AlunoIdId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Alunos");
        }
    }
}
