using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixAlunoUniqueIndexPartial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Alunos_UserId_AnoLetivoId",
                table: "Alunos");

            migrationBuilder.CreateIndex(
                name: "IX_Alunos_UserId_AnoLetivoId",
                table: "Alunos",
                columns: new[] { "UserId", "AnoLetivoId" },
                unique: true,
                filter: "\"Ativo\" = true");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Alunos_UserId_AnoLetivoId",
                table: "Alunos");

            migrationBuilder.CreateIndex(
                name: "IX_Alunos_UserId_AnoLetivoId",
                table: "Alunos",
                columns: new[] { "UserId", "AnoLetivoId" },
                unique: true);
        }
    }
}
