using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class CreateTablesAndRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserName",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "Elections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ElectionYear = table.Column<int>(type: "int", maxLength: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PartyGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartyGroups_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Municipalities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MunicipalityCode = table.Column<int>(type: "int", nullable: false),
                    ElectionAreaName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TotalSeatCount = table.Column<int>(type: "int", nullable: false),
                    ElectionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Municipalities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Municipalities_Elections_ElectionId",
                        column: x => x.ElectionId,
                        principalTable: "Elections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ElectionConstituencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FixedSeatCount = table.Column<int>(type: "int", nullable: false),
                    ElectionId = table.Column<int>(type: "int", nullable: false),
                    MunicipalityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectionConstituencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ElectionConstituencies_Elections_ElectionId",
                        column: x => x.ElectionId,
                        principalTable: "Elections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ElectionConstituencies_Municipalities_MunicipalityId",
                        column: x => x.MunicipalityId,
                        principalTable: "Municipalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OriginalElectionResultSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ElectionId = table.Column<int>(type: "int", nullable: false),
                    MunicipalityId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OriginalElectionResultSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OriginalElectionResultSets_Elections_ElectionId",
                        column: x => x.ElectionId,
                        principalTable: "Elections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OriginalElectionResultSets_Municipalities_MunicipalityId",
                        column: x => x.MunicipalityId,
                        principalTable: "Municipalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OriginalElectionResultSets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PoliticalParties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsLocal = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MunicipalityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoliticalParties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PoliticalParties_Municipalities_MunicipalityId",
                        column: x => x.MunicipalityId,
                        principalTable: "Municipalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PoliticalParties_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Scenarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OriginalElectionResultSetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scenarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scenarios_OriginalElectionResultSets_OriginalElectionResultSetId",
                        column: x => x.OriginalElectionResultSetId,
                        principalTable: "OriginalElectionResultSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Scenarios_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OriginalConstituencyVoteResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumberOfVotes = table.Column<int>(type: "int", nullable: false),
                    OriginalElectionResultSetId = table.Column<int>(type: "int", nullable: false),
                    ElectionConstituencyId = table.Column<int>(type: "int", nullable: false),
                    PoliticalPartyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OriginalConstituencyVoteResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OriginalConstituencyVoteResults_ElectionConstituencies_ElectionConstituencyId",
                        column: x => x.ElectionConstituencyId,
                        principalTable: "ElectionConstituencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OriginalConstituencyVoteResults_OriginalElectionResultSets_OriginalElectionResultSetId",
                        column: x => x.OriginalElectionResultSetId,
                        principalTable: "OriginalElectionResultSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OriginalConstituencyVoteResults_PoliticalParties_PoliticalPartyId",
                        column: x => x.PoliticalPartyId,
                        principalTable: "PoliticalParties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OriginalCouncilSeatAllocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AllocatedSeat = table.Column<int>(type: "int", nullable: false),
                    TotalSeatCountForPartyBeforeAllocation = table.Column<int>(type: "int", nullable: false),
                    ComparisonNumber = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    AllocationDivisor = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PoliticalPartyId = table.Column<int>(type: "int", nullable: false),
                    OriginalElectionResultSetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OriginalCouncilSeatAllocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OriginalCouncilSeatAllocations_OriginalElectionResultSets_OriginalElectionResultSetId",
                        column: x => x.OriginalElectionResultSetId,
                        principalTable: "OriginalElectionResultSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OriginalCouncilSeatAllocations_PoliticalParties_PoliticalPartyId",
                        column: x => x.PoliticalPartyId,
                        principalTable: "PoliticalParties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PartyGroupPoliticalParty",
                columns: table => new
                {
                    PartyGroupsId = table.Column<int>(type: "int", nullable: false),
                    PoliticalPartiesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyGroupPoliticalParty", x => new { x.PartyGroupsId, x.PoliticalPartiesId });
                    table.ForeignKey(
                        name: "FK_PartyGroupPoliticalParty_PartyGroups_PartyGroupsId",
                        column: x => x.PartyGroupsId,
                        principalTable: "PartyGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartyGroupPoliticalParty_PoliticalParties_PoliticalPartiesId",
                        column: x => x.PoliticalPartiesId,
                        principalTable: "PoliticalParties",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PartyGroupScenario",
                columns: table => new
                {
                    PartyGroupsId = table.Column<int>(type: "int", nullable: false),
                    ScenariosId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyGroupScenario", x => new { x.PartyGroupsId, x.ScenariosId });
                    table.ForeignKey(
                        name: "FK_PartyGroupScenario_PartyGroups_PartyGroupsId",
                        column: x => x.PartyGroupsId,
                        principalTable: "PartyGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartyGroupScenario_Scenarios_ScenariosId",
                        column: x => x.ScenariosId,
                        principalTable: "Scenarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ScenarioConstituencyVoteResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumberOfVotes = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PoliticalPartyId = table.Column<int>(type: "int", nullable: false),
                    ScenarioId = table.Column<int>(type: "int", nullable: false),
                    ElectionConstituencyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScenarioConstituencyVoteResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScenarioConstituencyVoteResults_ElectionConstituencies_ElectionConstituencyId",
                        column: x => x.ElectionConstituencyId,
                        principalTable: "ElectionConstituencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScenarioConstituencyVoteResults_PoliticalParties_PoliticalPartyId",
                        column: x => x.PoliticalPartyId,
                        principalTable: "PoliticalParties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScenarioConstituencyVoteResults_Scenarios_ScenarioId",
                        column: x => x.ScenarioId,
                        principalTable: "Scenarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScenarioCouncilSeatAllocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AllocatedSeat = table.Column<int>(type: "int", nullable: false),
                    TotalSeatCountForPartyBeforeAllocation = table.Column<int>(type: "int", nullable: false),
                    ComparisonNumber = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    AllocationDivisor = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ScenarioId = table.Column<int>(type: "int", nullable: false),
                    PoliticalPartyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScenarioCouncilSeatAllocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScenarioCouncilSeatAllocations_PoliticalParties_PoliticalPartyId",
                        column: x => x.PoliticalPartyId,
                        principalTable: "PoliticalParties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScenarioCouncilSeatAllocations_Scenarios_ScenarioId",
                        column: x => x.ScenarioId,
                        principalTable: "Scenarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email_UserName",
                table: "Users",
                columns: new[] { "Email", "UserName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ElectionConstituencies_ElectionId",
                table: "ElectionConstituencies",
                column: "ElectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ElectionConstituencies_MunicipalityId",
                table: "ElectionConstituencies",
                column: "MunicipalityId");

            migrationBuilder.CreateIndex(
                name: "IX_ElectionConstituencies_Name_ElectionId_MunicipalityId",
                table: "ElectionConstituencies",
                columns: new[] { "Name", "ElectionId", "MunicipalityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Elections_ElectionYear",
                table: "Elections",
                column: "ElectionYear",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Municipalities_ElectionId",
                table: "Municipalities",
                column: "ElectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Municipalities_MunicipalityCode_ElectionAreaName_ElectionId",
                table: "Municipalities",
                columns: new[] { "MunicipalityCode", "ElectionAreaName", "ElectionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OriginalConstituencyVoteResults_ElectionConstituencyId",
                table: "OriginalConstituencyVoteResults",
                column: "ElectionConstituencyId");

            migrationBuilder.CreateIndex(
                name: "IX_OriginalConstituencyVoteResults_OriginalElectionResultSetId_ElectionConstituencyId_PoliticalPartyId",
                table: "OriginalConstituencyVoteResults",
                columns: new[] { "OriginalElectionResultSetId", "ElectionConstituencyId", "PoliticalPartyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OriginalConstituencyVoteResults_PoliticalPartyId",
                table: "OriginalConstituencyVoteResults",
                column: "PoliticalPartyId");

            migrationBuilder.CreateIndex(
                name: "IX_OriginalCouncilSeatAllocations_AllocatedSeat_PoliticalPartyId_OriginalElectionResultSetId",
                table: "OriginalCouncilSeatAllocations",
                columns: new[] { "AllocatedSeat", "PoliticalPartyId", "OriginalElectionResultSetId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OriginalCouncilSeatAllocations_OriginalElectionResultSetId",
                table: "OriginalCouncilSeatAllocations",
                column: "OriginalElectionResultSetId");

            migrationBuilder.CreateIndex(
                name: "IX_OriginalCouncilSeatAllocations_PoliticalPartyId",
                table: "OriginalCouncilSeatAllocations",
                column: "PoliticalPartyId");

            migrationBuilder.CreateIndex(
                name: "IX_OriginalElectionResultSets_ElectionId",
                table: "OriginalElectionResultSets",
                column: "ElectionId");

            migrationBuilder.CreateIndex(
                name: "IX_OriginalElectionResultSets_MunicipalityId",
                table: "OriginalElectionResultSets",
                column: "MunicipalityId");

            migrationBuilder.CreateIndex(
                name: "IX_OriginalElectionResultSets_UserId_MunicipalityId_ElectionId",
                table: "OriginalElectionResultSets",
                columns: new[] { "UserId", "MunicipalityId", "ElectionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PartyGroupPoliticalParty_PoliticalPartiesId",
                table: "PartyGroupPoliticalParty",
                column: "PoliticalPartiesId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyGroups_UserId_Name",
                table: "PartyGroups",
                columns: new[] { "UserId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PartyGroupScenario_ScenariosId",
                table: "PartyGroupScenario",
                column: "ScenariosId");

            migrationBuilder.CreateIndex(
                name: "IX_PoliticalParties_MunicipalityId",
                table: "PoliticalParties",
                column: "MunicipalityId");

            migrationBuilder.CreateIndex(
                name: "IX_PoliticalParties_Name_MunicipalityId",
                table: "PoliticalParties",
                columns: new[] { "Name", "MunicipalityId" },
                unique: true,
                filter: "[MunicipalityId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PoliticalParties_UserId",
                table: "PoliticalParties",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ScenarioConstituencyVoteResults_ElectionConstituencyId",
                table: "ScenarioConstituencyVoteResults",
                column: "ElectionConstituencyId");

            migrationBuilder.CreateIndex(
                name: "IX_ScenarioConstituencyVoteResults_PoliticalPartyId_ScenarioId",
                table: "ScenarioConstituencyVoteResults",
                columns: new[] { "PoliticalPartyId", "ScenarioId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScenarioConstituencyVoteResults_ScenarioId",
                table: "ScenarioConstituencyVoteResults",
                column: "ScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ScenarioCouncilSeatAllocations_AllocatedSeat_ScenarioId_PoliticalPartyId",
                table: "ScenarioCouncilSeatAllocations",
                columns: new[] { "AllocatedSeat", "ScenarioId", "PoliticalPartyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScenarioCouncilSeatAllocations_PoliticalPartyId",
                table: "ScenarioCouncilSeatAllocations",
                column: "PoliticalPartyId");

            migrationBuilder.CreateIndex(
                name: "IX_ScenarioCouncilSeatAllocations_ScenarioId",
                table: "ScenarioCouncilSeatAllocations",
                column: "ScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Scenarios_Name_UserId",
                table: "Scenarios",
                columns: new[] { "Name", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Scenarios_OriginalElectionResultSetId",
                table: "Scenarios",
                column: "OriginalElectionResultSetId");

            migrationBuilder.CreateIndex(
                name: "IX_Scenarios_UserId",
                table: "Scenarios",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OriginalConstituencyVoteResults");

            migrationBuilder.DropTable(
                name: "OriginalCouncilSeatAllocations");

            migrationBuilder.DropTable(
                name: "PartyGroupPoliticalParty");

            migrationBuilder.DropTable(
                name: "PartyGroupScenario");

            migrationBuilder.DropTable(
                name: "ScenarioConstituencyVoteResults");

            migrationBuilder.DropTable(
                name: "ScenarioCouncilSeatAllocations");

            migrationBuilder.DropTable(
                name: "PartyGroups");

            migrationBuilder.DropTable(
                name: "ElectionConstituencies");

            migrationBuilder.DropTable(
                name: "PoliticalParties");

            migrationBuilder.DropTable(
                name: "Scenarios");

            migrationBuilder.DropTable(
                name: "OriginalElectionResultSets");

            migrationBuilder.DropTable(
                name: "Municipalities");

            migrationBuilder.DropTable(
                name: "Elections");

            migrationBuilder.DropIndex(
                name: "IX_Users_Email_UserName",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);
        }
    }
}
