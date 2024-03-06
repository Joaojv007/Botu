using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class ExclusaoEmCascata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Avaliacoes_Disciplinas_DisciplinaId",
                table: "Avaliacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Disciplinas_Semestres_SemestreId",
                table: "Disciplinas");

            migrationBuilder.DropForeignKey(
                name: "FK_Semestres_Cursos_CursoId",
                table: "Semestres");

            migrationBuilder.AlterColumn<Guid>(
                name: "CursoId",
                table: "Semestres",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "SemestreId",
                table: "Disciplinas",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "DisciplinaId",
                table: "Avaliacoes",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddForeignKey(
                name: "FK_Avaliacoes_Disciplinas_DisciplinaId",
                table: "Avaliacoes",
                column: "DisciplinaId",
                principalTable: "Disciplinas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Disciplinas_Semestres_SemestreId",
                table: "Disciplinas",
                column: "SemestreId",
                principalTable: "Semestres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Semestres_Cursos_CursoId",
                table: "Semestres",
                column: "CursoId",
                principalTable: "Cursos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Avaliacoes_Disciplinas_DisciplinaId",
                table: "Avaliacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Disciplinas_Semestres_SemestreId",
                table: "Disciplinas");

            migrationBuilder.DropForeignKey(
                name: "FK_Semestres_Cursos_CursoId",
                table: "Semestres");

            migrationBuilder.AlterColumn<Guid>(
                name: "CursoId",
                table: "Semestres",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "SemestreId",
                table: "Disciplinas",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "DisciplinaId",
                table: "Avaliacoes",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddForeignKey(
                name: "FK_Avaliacoes_Disciplinas_DisciplinaId",
                table: "Avaliacoes",
                column: "DisciplinaId",
                principalTable: "Disciplinas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Disciplinas_Semestres_SemestreId",
                table: "Disciplinas",
                column: "SemestreId",
                principalTable: "Semestres",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Semestres_Cursos_CursoId",
                table: "Semestres",
                column: "CursoId",
                principalTable: "Cursos",
                principalColumn: "Id");
        }
    }
}
