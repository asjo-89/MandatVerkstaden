using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class MoveColumnFromTableMunicipalitiesToOriginalElectionResultSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalSeatCount",
                table: "Municipalities");

            migrationBuilder.AddColumn<int>(
                name: "TotalCouncilSeatCount",
                table: "OriginalElectionResultSets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalCouncilSeatCount",
                table: "OriginalElectionResultSets");

            migrationBuilder.AddColumn<int>(
                name: "TotalSeatCount",
                table: "Municipalities",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
