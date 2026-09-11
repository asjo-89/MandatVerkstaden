using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnToTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalSeatCountForPartyBeforeAllocation",
                table: "ScenarioCouncilSeatAllocations",
                newName: "TotalCouncilSeatCountForParty");

            migrationBuilder.AddColumn<bool>(
                name: "WonSeat",
                table: "OriginalBoardSeatAllocations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WonSeat",
                table: "OriginalBoardSeatAllocations");

            migrationBuilder.RenameColumn(
                name: "TotalCouncilSeatCountForParty",
                table: "ScenarioCouncilSeatAllocations",
                newName: "TotalSeatCountForPartyBeforeAllocation");
        }
    }
}
