using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiC_.Migrations
{
    /// <inheritdoc />
    public partial class ApplyFieldIsUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Turmas_Codigo",
                table: "Turmas",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Alunos_Email",
                table: "Alunos",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Turmas_Codigo",
                table: "Turmas");

            migrationBuilder.DropIndex(
                name: "IX_Alunos_Email",
                table: "Alunos");
        }
    }
}
