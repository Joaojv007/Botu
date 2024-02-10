using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class AjusteTabelaFaculdade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Media",
                table: "Disciplinas",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "FaculdadeAluno",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FaculdadeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AlunoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaculdadeAluno", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FaculdadeAluno_Alunos_AlunoId",
                        column: x => x.AlunoId,
                        principalTable: "Alunos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FaculdadeAluno_Faculdades_FaculdadeId",
                        column: x => x.FaculdadeId,
                        principalTable: "Faculdades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_FaculdadeAluno_AlunoId",
                table: "FaculdadeAluno",
                column: "AlunoId");

            migrationBuilder.CreateIndex(
                name: "IX_FaculdadeAluno_FaculdadeId",
                table: "FaculdadeAluno",
                column: "FaculdadeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FaculdadeAluno");

            migrationBuilder.DropColumn(
                name: "Media",
                table: "Disciplinas");
        }
    }
}
