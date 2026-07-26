using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnToMunicipalityAndConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VotingDistrictsCount",
                table: "Municipalities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Municipality_VotingDistrictsCount_Positive",
                table: "Municipalities",
                sql: "[VotingDistrictsCount] >= 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Municipality_VotingDistrictsCount_Positive",
                table: "Municipalities");

            migrationBuilder.DropColumn(
                name: "VotingDistrictsCount",
                table: "Municipalities");
        }
    }
}
