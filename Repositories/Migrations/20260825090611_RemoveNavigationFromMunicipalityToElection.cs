using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNavigationFromMunicipalityToElection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Municipalities_Elections_ElectionId",
                table: "Municipalities");

            migrationBuilder.DropIndex(
                name: "IX_Municipalities_ElectionId",
                table: "Municipalities");

            migrationBuilder.DropIndex(
                name: "IX_Municipalities_MunicipalityCode_ElectionAreaName_ElectionId",
                table: "Municipalities");

            migrationBuilder.DropColumn(
                name: "ElectionId",
                table: "Municipalities");

            migrationBuilder.CreateIndex(
                name: "IX_Municipalities_MunicipalityCode_ElectionAreaName",
                table: "Municipalities",
                columns: new[] { "MunicipalityCode", "ElectionAreaName" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Municipalities_MunicipalityCode_ElectionAreaName",
                table: "Municipalities");

            migrationBuilder.AddColumn<int>(
                name: "ElectionId",
                table: "Municipalities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Municipalities_ElectionId",
                table: "Municipalities",
                column: "ElectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Municipalities_MunicipalityCode_ElectionAreaName_ElectionId",
                table: "Municipalities",
                columns: new[] { "MunicipalityCode", "ElectionAreaName", "ElectionId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Municipalities_Elections_ElectionId",
                table: "Municipalities",
                column: "ElectionId",
                principalTable: "Elections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
