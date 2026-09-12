using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUniquIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OriginalElectionResultSets_UserId_MunicipalityId_ElectionId",
                table: "OriginalElectionResultSets");

            migrationBuilder.CreateIndex(
                name: "IX_OriginalElectionResultSets_UserId",
                table: "OriginalElectionResultSets",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OriginalElectionResultSets_UserId",
                table: "OriginalElectionResultSets");

            migrationBuilder.CreateIndex(
                name: "IX_OriginalElectionResultSets_UserId_MunicipalityId_ElectionId",
                table: "OriginalElectionResultSets",
                columns: new[] { "UserId", "MunicipalityId", "ElectionId" },
                unique: true);
        }
    }
}
