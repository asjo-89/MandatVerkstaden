using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddDataToTablePoliticalParties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PoliticalParties",
                columns: new[] { "Id", "IsLocal", "PartyName" },
                values: new object[,]
                {
                    { 1, false, "Arbetarepartiet-Socialdemokraterna" },
                    { 2, false, "Moderaterna" },
                    { 3, false, "Sverigedemokraterna" },
                    { 4, false, "Centerpartiet" },
                    { 5, false, "Vänsterpartiet" },
                    { 6, false, "Kristdemokraterna" },
                    { 7, false, "Liberalerna" },
                    { 8, false, "Miljöpartiet de gröna" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PoliticalParties",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PoliticalParties",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PoliticalParties",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PoliticalParties",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PoliticalParties",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "PoliticalParties",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "PoliticalParties",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "PoliticalParties",
                keyColumn: "Id",
                keyValue: 8);
        }
    }
}
