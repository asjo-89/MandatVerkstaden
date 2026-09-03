using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class CreateTablesForBoardSeatAllocations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "AllocationDivisor",
                table: "OriginalCouncilSeatAllocations",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldPrecision: 18,
                oldScale: 4);

            migrationBuilder.CreateTable(
                name: "OriginalBoardSeatAllocationSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaxSeatCount = table.Column<int>(type: "int", nullable: false),
                    OriginalElectionResultSetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OriginalBoardSeatAllocationSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OriginalBoardSeatAllocationSets_OriginalElectionResultSets_OriginalElectionResultSetId",
                        column: x => x.OriginalElectionResultSetId,
                        principalTable: "OriginalElectionResultSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OriginalBoardSeatAllocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeatAllocationStep = table.Column<int>(type: "int", nullable: false),
                    ComparisonNumber = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    AllocationDivisor = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    WonByLotDrawing = table.Column<bool>(type: "bit", nullable: false),
                    LotDrawingGroupId = table.Column<int>(type: "int", nullable: false),
                    OriginalBoardSeatAllocationSetId = table.Column<int>(type: "int", nullable: false),
                    PoliticalPartyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OriginalBoardSeatAllocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OriginalBoardSeatAllocations_OriginalBoardSeatAllocationSets_OriginalBoardSeatAllocationSetId",
                        column: x => x.OriginalBoardSeatAllocationSetId,
                        principalTable: "OriginalBoardSeatAllocationSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OriginalBoardSeatAllocations_PoliticalParties_PoliticalPartyId",
                        column: x => x.PoliticalPartyId,
                        principalTable: "PoliticalParties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OriginalBoardSeatAllocations_OriginalBoardSeatAllocationSetId_SeatAllocationStep_PoliticalPartyId",
                table: "OriginalBoardSeatAllocations",
                columns: new[] { "OriginalBoardSeatAllocationSetId", "SeatAllocationStep", "PoliticalPartyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OriginalBoardSeatAllocations_PoliticalPartyId",
                table: "OriginalBoardSeatAllocations",
                column: "PoliticalPartyId");

            migrationBuilder.CreateIndex(
                name: "IX_OriginalBoardSeatAllocationSets_OriginalElectionResultSetId",
                table: "OriginalBoardSeatAllocationSets",
                column: "OriginalElectionResultSetId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OriginalBoardSeatAllocationSets_OriginalElectionResultSetId_MaxSeatCount",
                table: "OriginalBoardSeatAllocationSets",
                columns: new[] { "OriginalElectionResultSetId", "MaxSeatCount" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OriginalBoardSeatAllocations");

            migrationBuilder.DropTable(
                name: "OriginalBoardSeatAllocationSets");

            migrationBuilder.AlterColumn<decimal>(
                name: "AllocationDivisor",
                table: "OriginalCouncilSeatAllocations",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);
        }
    }
}
