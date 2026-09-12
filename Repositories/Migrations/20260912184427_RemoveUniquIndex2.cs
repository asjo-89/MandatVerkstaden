using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUniquIndex2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OriginalBoardSeatAllocationSets_OriginalElectionResultSetId_MaxSeatCount",
                table: "OriginalBoardSeatAllocationSets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_OriginalBoardSeatAllocationSets_OriginalElectionResultSetId_MaxSeatCount",
                table: "OriginalBoardSeatAllocationSets",
                columns: new[] { "OriginalElectionResultSetId", "MaxSeatCount" },
                unique: true);
        }
    }
}
