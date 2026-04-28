using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SchoolAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnosLetivos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ano = table.Column<int>(type: "integer", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnosLetivos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Disciplinas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disciplinas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Turmas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Segmento = table.Column<string>(type: "text", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    AnoLetivoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turmas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Turmas_AnosLetivos_AnoLetivoId",
                        column: x => x.AnoLetivoId,
                        principalTable: "AnosLetivos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Alunos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    NumeroChamada = table.Column<int>(type: "integer", nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    TurmaId = table.Column<int>(type: "integer", nullable: false),
                    AnoLetivoId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alunos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alunos_AnosLetivos_AnoLetivoId",
                        column: x => x.AnoLetivoId,
                        principalTable: "AnosLetivos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alunos_Turmas_TurmaId",
                        column: x => x.TurmaId,
                        principalTable: "Turmas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alunos_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Frequencias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Data = table.Column<DateOnly>(type: "date", nullable: false),
                    TurmaId = table.Column<int>(type: "integer", nullable: false),
                    AnoLetivoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Frequencias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Frequencias_AnosLetivos_AnoLetivoId",
                        column: x => x.AnoLetivoId,
                        principalTable: "AnosLetivos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Frequencias_Turmas_TurmaId",
                        column: x => x.TurmaId,
                        principalTable: "Turmas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TurmaDisciplinaProfessores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TurmaId = table.Column<int>(type: "integer", nullable: false),
                    DisciplinaId = table.Column<int>(type: "integer", nullable: false),
                    ProfessorId = table.Column<int>(type: "integer", nullable: false),
                    AnoLetivoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TurmaDisciplinaProfessores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TurmaDisciplinaProfessores_AnosLetivos_AnoLetivoId",
                        column: x => x.AnoLetivoId,
                        principalTable: "AnosLetivos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TurmaDisciplinaProfessores_Disciplinas_DisciplinaId",
                        column: x => x.DisciplinaId,
                        principalTable: "Disciplinas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TurmaDisciplinaProfessores_Turmas_TurmaId",
                        column: x => x.TurmaId,
                        principalTable: "Turmas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TurmaDisciplinaProfessores_Users_ProfessorId",
                        column: x => x.ProfessorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DesenvolvimentosMaternal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Bimestre = table.Column<int>(type: "integer", nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: false),
                    AlunoId = table.Column<int>(type: "integer", nullable: false),
                    TurmaId = table.Column<int>(type: "integer", nullable: false),
                    AnoLetivoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DesenvolvimentosMaternal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DesenvolvimentosMaternal_Alunos_AlunoId",
                        column: x => x.AlunoId,
                        principalTable: "Alunos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DesenvolvimentosMaternal_AnosLetivos_AnoLetivoId",
                        column: x => x.AnoLetivoId,
                        principalTable: "AnosLetivos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DesenvolvimentosMaternal_Turmas_TurmaId",
                        column: x => x.TurmaId,
                        principalTable: "Turmas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Unidade = table.Column<int>(type: "integer", nullable: false),
                    Valor = table.Column<decimal>(type: "numeric", nullable: false),
                    AlunoId = table.Column<int>(type: "integer", nullable: false),
                    DisciplinaId = table.Column<int>(type: "integer", nullable: false),
                    TurmaId = table.Column<int>(type: "integer", nullable: false),
                    AnoLetivoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notas_Alunos_AlunoId",
                        column: x => x.AlunoId,
                        principalTable: "Alunos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notas_AnosLetivos_AnoLetivoId",
                        column: x => x.AnoLetivoId,
                        principalTable: "AnosLetivos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notas_Disciplinas_DisciplinaId",
                        column: x => x.DisciplinaId,
                        principalTable: "Disciplinas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notas_Turmas_TurmaId",
                        column: x => x.TurmaId,
                        principalTable: "Turmas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FrequenciaAlunos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Presente = table.Column<bool>(type: "boolean", nullable: false),
                    FrequenciaId = table.Column<int>(type: "integer", nullable: false),
                    AlunoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrequenciaAlunos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FrequenciaAlunos_Alunos_AlunoId",
                        column: x => x.AlunoId,
                        principalTable: "Alunos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FrequenciaAlunos_Frequencias_FrequenciaId",
                        column: x => x.FrequenciaId,
                        principalTable: "Frequencias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alunos_AnoLetivoId",
                table: "Alunos",
                column: "AnoLetivoId");

            migrationBuilder.CreateIndex(
                name: "IX_Alunos_TurmaId",
                table: "Alunos",
                column: "TurmaId");

            migrationBuilder.CreateIndex(
                name: "IX_Alunos_UserId",
                table: "Alunos",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DesenvolvimentosMaternal_AlunoId_Bimestre_AnoLetivoId",
                table: "DesenvolvimentosMaternal",
                columns: new[] { "AlunoId", "Bimestre", "AnoLetivoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DesenvolvimentosMaternal_AnoLetivoId",
                table: "DesenvolvimentosMaternal",
                column: "AnoLetivoId");

            migrationBuilder.CreateIndex(
                name: "IX_DesenvolvimentosMaternal_TurmaId",
                table: "DesenvolvimentosMaternal",
                column: "TurmaId");

            migrationBuilder.CreateIndex(
                name: "IX_FrequenciaAlunos_AlunoId",
                table: "FrequenciaAlunos",
                column: "AlunoId");

            migrationBuilder.CreateIndex(
                name: "IX_FrequenciaAlunos_FrequenciaId",
                table: "FrequenciaAlunos",
                column: "FrequenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Frequencias_AnoLetivoId",
                table: "Frequencias",
                column: "AnoLetivoId");

            migrationBuilder.CreateIndex(
                name: "IX_Frequencias_TurmaId_Data",
                table: "Frequencias",
                columns: new[] { "TurmaId", "Data" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notas_AlunoId_DisciplinaId_Unidade_AnoLetivoId",
                table: "Notas",
                columns: new[] { "AlunoId", "DisciplinaId", "Unidade", "AnoLetivoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notas_AnoLetivoId",
                table: "Notas",
                column: "AnoLetivoId");

            migrationBuilder.CreateIndex(
                name: "IX_Notas_DisciplinaId",
                table: "Notas",
                column: "DisciplinaId");

            migrationBuilder.CreateIndex(
                name: "IX_Notas_TurmaId",
                table: "Notas",
                column: "TurmaId");

            migrationBuilder.CreateIndex(
                name: "IX_TurmaDisciplinaProfessores_AnoLetivoId",
                table: "TurmaDisciplinaProfessores",
                column: "AnoLetivoId");

            migrationBuilder.CreateIndex(
                name: "IX_TurmaDisciplinaProfessores_DisciplinaId",
                table: "TurmaDisciplinaProfessores",
                column: "DisciplinaId");

            migrationBuilder.CreateIndex(
                name: "IX_TurmaDisciplinaProfessores_ProfessorId",
                table: "TurmaDisciplinaProfessores",
                column: "ProfessorId");

            migrationBuilder.CreateIndex(
                name: "IX_TurmaDisciplinaProfessores_TurmaId_DisciplinaId_AnoLetivoId",
                table: "TurmaDisciplinaProfessores",
                columns: new[] { "TurmaId", "DisciplinaId", "AnoLetivoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Turmas_AnoLetivoId",
                table: "Turmas",
                column: "AnoLetivoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DesenvolvimentosMaternal");

            migrationBuilder.DropTable(
                name: "FrequenciaAlunos");

            migrationBuilder.DropTable(
                name: "Notas");

            migrationBuilder.DropTable(
                name: "TurmaDisciplinaProfessores");

            migrationBuilder.DropTable(
                name: "Frequencias");

            migrationBuilder.DropTable(
                name: "Alunos");

            migrationBuilder.DropTable(
                name: "Disciplinas");

            migrationBuilder.DropTable(
                name: "Turmas");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "AnosLetivos");
        }
    }
}
