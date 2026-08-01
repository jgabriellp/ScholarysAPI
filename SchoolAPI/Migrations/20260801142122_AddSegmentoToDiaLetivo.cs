using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddSegmentoToDiaLetivo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DiasLetivos_AnoLetivoId_Data",
                table: "DiasLetivos");

            migrationBuilder.AddColumn<string>(
                name: "Segmento",
                table: "DiasLetivos",
                type: "text",
                nullable: false,
                defaultValue: "Fundamental");

            migrationBuilder.CreateIndex(
                name: "IX_DiasLetivos_AnoLetivoId_Data_Segmento",
                table: "DiasLetivos",
                columns: new[] { "AnoLetivoId", "Data", "Segmento" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DiasLetivos_AnoLetivoId_Data_Segmento",
                table: "DiasLetivos");

            migrationBuilder.DropColumn(
                name: "Segmento",
                table: "DiasLetivos");

            migrationBuilder.CreateIndex(
                name: "IX_DiasLetivos_AnoLetivoId_Data",
                table: "DiasLetivos",
                columns: new[] { "AnoLetivoId", "Data" },
                unique: true);
        }
    }
}
