using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SchoolAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddDiaLetivoRelatoAula : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiasLetivos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Data = table.Column<DateOnly>(type: "date", nullable: false),
                    AnoLetivoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiasLetivos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiasLetivos_AnosLetivos_AnoLetivoId",
                        column: x => x.AnoLetivoId,
                        principalTable: "AnosLetivos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RelatosAula",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descricao = table.Column<string>(type: "text", nullable: false),
                    DiaLetivoId = table.Column<int>(type: "integer", nullable: false),
                    TurmaId = table.Column<int>(type: "integer", nullable: false),
                    ProfessorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatosAula", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RelatosAula_DiasLetivos_DiaLetivoId",
                        column: x => x.DiaLetivoId,
                        principalTable: "DiasLetivos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RelatosAula_Turmas_TurmaId",
                        column: x => x.TurmaId,
                        principalTable: "Turmas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RelatosAula_Users_ProfessorId",
                        column: x => x.ProfessorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiasLetivos_AnoLetivoId_Data",
                table: "DiasLetivos",
                columns: new[] { "AnoLetivoId", "Data" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RelatosAula_DiaLetivoId_TurmaId_ProfessorId",
                table: "RelatosAula",
                columns: new[] { "DiaLetivoId", "TurmaId", "ProfessorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RelatosAula_ProfessorId",
                table: "RelatosAula",
                column: "ProfessorId");

            migrationBuilder.CreateIndex(
                name: "IX_RelatosAula_TurmaId",
                table: "RelatosAula",
                column: "TurmaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RelatosAula");

            migrationBuilder.DropTable(
                name: "DiasLetivos");
        }
    }
}
