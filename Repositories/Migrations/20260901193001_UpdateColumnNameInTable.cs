using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class UpdateColumnNameInTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalSeatCountForPartyBeforeAllocation",
                table: "OriginalCouncilSeatAllocations",
                newName: "TotalCouncilSeatCountForParty");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalCouncilSeatCountForParty",
                table: "OriginalCouncilSeatAllocations",
                newName: "TotalSeatCountForPartyBeforeAllocation");
        }
    }
}
