using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Elections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ElectionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Municipalities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CouncilSeatCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Municipalities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PoliticalParties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsLocal = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoliticalParties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: false),
                    HashedPassword = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FailedLogIns = table.Column<int>(type: "int", nullable: false),
                    IsLockedOut = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ElectionResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumberOfVotes = table.Column<int>(type: "int", nullable: false),
                    VotePercentage = table.Column<decimal>(type: "decimal(7,2)", precision: 7, scale: 2, nullable: false),
                    MunicipalityId = table.Column<int>(type: "int", nullable: false),
                    PoliticalPartyId = table.Column<int>(type: "int", nullable: false),
                    ElectionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectionResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ElectionResults_Elections_ElectionId",
                        column: x => x.ElectionId,
                        principalTable: "Elections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ElectionResults_Municipalities_MunicipalityId",
                        column: x => x.MunicipalityId,
                        principalTable: "Municipalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ElectionResults_PoliticalParties_PoliticalPartyId",
                        column: x => x.PoliticalPartyId,
                        principalTable: "PoliticalParties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MunicipalityPoliticalParty",
                columns: table => new
                {
                    MunicipalitiesId = table.Column<int>(type: "int", nullable: false),
                    PoliticalPartiesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MunicipalityPoliticalParty", x => new { x.MunicipalitiesId, x.PoliticalPartiesId });
                    table.ForeignKey(
                        name: "FK_MunicipalityPoliticalParty_Municipalities_MunicipalitiesId",
                        column: x => x.MunicipalitiesId,
                        principalTable: "Municipalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MunicipalityPoliticalParty_PoliticalParties_PoliticalPartiesId",
                        column: x => x.PoliticalPartiesId,
                        principalTable: "PoliticalParties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CouncilSeatAllocationsScenarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScenarioName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SeatNumber = table.Column<int>(type: "int", nullable: false),
                    PartySeatCountBeforeAllocation = table.Column<int>(type: "int", nullable: false),
                    ComparisonNumber = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    AllocationDivisor = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    VoteCountUsed = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MunicipalityId = table.Column<int>(type: "int", nullable: false),
                    ElectionId = table.Column<int>(type: "int", nullable: false),
                    PoliticalPartyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CouncilSeatAllocationsScenarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CouncilSeatAllocationsScenarios_Elections_ElectionId",
                        column: x => x.ElectionId,
                        principalTable: "Elections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CouncilSeatAllocationsScenarios_Municipalities_MunicipalityId",
                        column: x => x.MunicipalityId,
                        principalTable: "Municipalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CouncilSeatAllocationsScenarios_PoliticalParties_PoliticalPartyId",
                        column: x => x.PoliticalPartyId,
                        principalTable: "PoliticalParties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CouncilSeatAllocationsScenarios_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CouncilSeatAllocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeatNumber = table.Column<int>(type: "int", nullable: false),
                    PartySeatCountBeforeAllocation = table.Column<int>(type: "int", nullable: false),
                    ComparisonNumber = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    AllocationDivisor = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    MunicipalityId = table.Column<int>(type: "int", nullable: false),
                    ElectionId = table.Column<int>(type: "int", nullable: false),
                    ElectionResultId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CouncilSeatAllocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CouncilSeatAllocations_ElectionResults_ElectionResultId",
                        column: x => x.ElectionResultId,
                        principalTable: "ElectionResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CouncilSeatAllocations_Elections_ElectionId",
                        column: x => x.ElectionId,
                        principalTable: "Elections",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CouncilSeatAllocations_Municipalities_MunicipalityId",
                        column: x => x.MunicipalityId,
                        principalTable: "Municipalities",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Municipalities",
                columns: new[] { "Id", "CouncilSeatCount", "Name" },
                values: new object[,]
                {
                    { 1, 0, "Ale" },
                    { 2, 0, "Alingsås" },
                    { 3, 0, "Alvesta" },
                    { 4, 0, "Aneby" },
                    { 5, 0, "Arboga" },
                    { 6, 0, "Arjeplog" },
                    { 7, 0, "Arvidsjaur" },
                    { 8, 0, "Arvika" },
                    { 9, 0, "Askersund" },
                    { 10, 0, "Avesta" },
                    { 11, 0, "Bengtsfors" },
                    { 12, 0, "Berg" },
                    { 13, 0, "Bjurholm" },
                    { 14, 0, "Bjuv" },
                    { 15, 0, "Boden" },
                    { 16, 0, "Bollebygd" },
                    { 17, 0, "Bollnäs" },
                    { 18, 0, "Borgholm" },
                    { 19, 0, "Borlänge" },
                    { 20, 0, "Borås" },
                    { 21, 0, "Botkyrka" },
                    { 22, 0, "Boxholm" },
                    { 23, 0, "Bromölla" },
                    { 24, 0, "Bräcke" },
                    { 25, 0, "Burlöv" },
                    { 26, 0, "Båstad" },
                    { 27, 0, "Dals-Ed" },
                    { 28, 0, "Danderyd" },
                    { 29, 0, "Degerfors" },
                    { 30, 0, "Dorotea" },
                    { 31, 0, "Eda" },
                    { 32, 0, "Ekerö" },
                    { 33, 0, "Eksjö" },
                    { 34, 0, "Emmaboda" },
                    { 35, 0, "Enköping" },
                    { 36, 0, "Eskilstuna" },
                    { 37, 0, "Eslöv" },
                    { 38, 0, "Essunga" },
                    { 39, 0, "Fagersta" },
                    { 40, 0, "Falun" },
                    { 41, 0, "Filipstad" },
                    { 42, 0, "Finspång" },
                    { 43, 0, "Flen" },
                    { 44, 0, "Forshaga" },
                    { 45, 0, "Gagnef" },
                    { 46, 0, "Gislaved" },
                    { 47, 0, "Gnesta" },
                    { 48, 0, "Gnosjö" },
                    { 49, 0, "Gotland" },
                    { 50, 0, "Grums" },
                    { 51, 0, "Grästorp" },
                    { 52, 0, "Gullspång" },
                    { 53, 0, "Gällivare" },
                    { 54, 0, "Gävle" },
                    { 55, 0, "Göteborg" },
                    { 56, 0, "Götene" },
                    { 57, 0, "Habo" },
                    { 58, 0, "Hagfors" },
                    { 59, 0, "Hallsberg" },
                    { 60, 0, "Hallstahammar" },
                    { 61, 0, "Halmstad" },
                    { 62, 0, "Hammarö" },
                    { 63, 0, "Haninge" },
                    { 64, 0, "Haparanda" },
                    { 65, 0, "Heby" },
                    { 66, 0, "Hedemora" },
                    { 67, 0, "Helsingborg" },
                    { 68, 0, "Herrljunga" },
                    { 69, 0, "Hjo" },
                    { 70, 0, "Hofors" },
                    { 71, 0, "Huddinge" },
                    { 72, 0, "Hudiksvall" },
                    { 73, 0, "Hultsfred" },
                    { 74, 0, "Hylte" },
                    { 75, 0, "Håbo" },
                    { 76, 0, "Hällefors" },
                    { 77, 0, "Härjedalen" },
                    { 78, 0, "Härnösand" },
                    { 79, 0, "Härryda" },
                    { 80, 0, "Hässleholm" },
                    { 81, 0, "Höganäs" },
                    { 82, 0, "Högsby" },
                    { 83, 0, "Hörby" },
                    { 84, 0, "Höör" },
                    { 85, 0, "Jokkmokk" },
                    { 86, 0, "Järfälla" },
                    { 87, 0, "Jönköping" },
                    { 88, 0, "Kalix" },
                    { 89, 0, "Kalmar" },
                    { 90, 0, "Karlsborg" },
                    { 91, 0, "Karlshamn" },
                    { 92, 0, "Karlskoga" },
                    { 93, 0, "Karlskrona" },
                    { 94, 0, "Karlstad" },
                    { 95, 0, "Katrineholm" },
                    { 96, 0, "Kil" },
                    { 97, 0, "Kinda" },
                    { 98, 0, "Kiruna" },
                    { 99, 0, "Klippan" },
                    { 100, 0, "Knivsta" },
                    { 101, 0, "Kramfors" },
                    { 102, 0, "Kristianstad" },
                    { 103, 0, "Kristinehamn" },
                    { 104, 0, "Krokom" },
                    { 105, 0, "Kumla" },
                    { 106, 0, "Kungsbacka" },
                    { 107, 0, "Kungsör" },
                    { 108, 0, "Kungälv" },
                    { 109, 0, "Kävlinge" },
                    { 110, 0, "Köping" },
                    { 111, 0, "Laholm" },
                    { 112, 0, "Landskrona" },
                    { 113, 0, "Laxå" },
                    { 114, 0, "Lekeberg" },
                    { 115, 0, "Leksand" },
                    { 116, 0, "Lerum" },
                    { 117, 0, "Lessebo" },
                    { 118, 0, "Lidingö" },
                    { 119, 0, "Lidköping" },
                    { 120, 0, "Lilla Edet" },
                    { 121, 0, "Lindesberg" },
                    { 122, 0, "Linköping" },
                    { 123, 0, "Ljungby" },
                    { 124, 0, "Ljusdal" },
                    { 125, 0, "Ljusnarsberg" },
                    { 126, 0, "Lomma" },
                    { 127, 0, "Ludvika" },
                    { 128, 0, "Luleå" },
                    { 129, 0, "Lund" },
                    { 130, 0, "Lycksele" },
                    { 131, 0, "Lysekil" },
                    { 132, 0, "Malmö" },
                    { 133, 0, "Malung-Sälen" },
                    { 134, 0, "Malå" },
                    { 135, 0, "Mariestad" },
                    { 136, 0, "Mark" },
                    { 137, 0, "Markaryd" },
                    { 138, 0, "Mellerud" },
                    { 139, 0, "Mjölby" },
                    { 140, 0, "Mora" },
                    { 141, 0, "Motala" },
                    { 142, 0, "Mullsjö" },
                    { 143, 0, "Munkedal" },
                    { 144, 0, "Munkfors" },
                    { 145, 0, "Mölndal" },
                    { 146, 0, "Mönsterås" },
                    { 147, 0, "Mörbylånga" },
                    { 148, 0, "Nacka" },
                    { 149, 0, "Nora" },
                    { 150, 0, "Norberg" },
                    { 151, 0, "Nordanstig" },
                    { 152, 0, "Nordmaling" },
                    { 153, 0, "Norrköping" },
                    { 154, 0, "Norrtälje" },
                    { 155, 0, "Norsjö" },
                    { 156, 0, "Nybro" },
                    { 157, 0, "Nykvarn" },
                    { 158, 0, "Nyköping" },
                    { 159, 0, "Nynäshamn" },
                    { 160, 0, "Nässjö" },
                    { 161, 0, "Ockelbo" },
                    { 162, 0, "Olofström" },
                    { 163, 0, "Orsa" },
                    { 164, 0, "Orust" },
                    { 165, 0, "Osby" },
                    { 166, 0, "Oskarshamn" },
                    { 167, 0, "Ovanåker" },
                    { 168, 0, "Oxelösund" },
                    { 169, 0, "Pajala" },
                    { 170, 0, "Partille" },
                    { 171, 0, "Perstorp" },
                    { 172, 0, "Piteå" },
                    { 173, 0, "Ragunda" },
                    { 174, 0, "Robertsfors" },
                    { 175, 0, "Ronneby" },
                    { 176, 0, "Rättvik" },
                    { 177, 0, "Sala" },
                    { 178, 0, "Salem" },
                    { 179, 0, "Sandviken" },
                    { 180, 0, "Sigtuna" },
                    { 181, 0, "Simrishamn" },
                    { 182, 0, "Sjöbo" },
                    { 183, 0, "Skara" },
                    { 184, 0, "Skellefteå" },
                    { 185, 0, "Skinnskatteberg" },
                    { 186, 0, "Skurup" },
                    { 187, 0, "Skövde" },
                    { 188, 0, "Smedjebacken" },
                    { 189, 0, "Sollefteå" },
                    { 190, 0, "Sollentuna" },
                    { 191, 0, "Solna" },
                    { 192, 0, "Sorsele" },
                    { 193, 0, "Sotenäs" },
                    { 194, 0, "Staffanstorp" },
                    { 195, 0, "Stenungsund" },
                    { 196, 0, "Stockholm" },
                    { 197, 0, "Storfors" },
                    { 198, 0, "Storuman" },
                    { 199, 0, "Strängnäs" },
                    { 200, 0, "Strömstad" },
                    { 201, 0, "Strömsund" },
                    { 202, 0, "Sundbyberg" },
                    { 203, 0, "Sundsvall" },
                    { 204, 0, "Sunne" },
                    { 205, 0, "Surahammar" },
                    { 206, 0, "Svalöv" },
                    { 207, 0, "Svedala" },
                    { 208, 0, "Svenljunga" },
                    { 209, 0, "Säffle" },
                    { 210, 0, "Säter" },
                    { 211, 0, "Sävsjö" },
                    { 212, 0, "Söderhamn" },
                    { 213, 0, "Söderköping" },
                    { 214, 0, "Södertälje" },
                    { 215, 0, "Sölvesborg" },
                    { 216, 0, "Tanum" },
                    { 217, 0, "Tibro" },
                    { 218, 0, "Tidaholm" },
                    { 219, 0, "Tierp" },
                    { 220, 0, "Timrå" },
                    { 221, 0, "Tingsryd" },
                    { 222, 0, "Tjörn" },
                    { 223, 0, "Tomelilla" },
                    { 224, 0, "Torsby" },
                    { 225, 0, "Torsås" },
                    { 226, 0, "Tranemo" },
                    { 227, 0, "Tranås" },
                    { 228, 0, "Trelleborg" },
                    { 229, 0, "Trollhättan" },
                    { 230, 0, "Trosa" },
                    { 231, 0, "Tyresö" },
                    { 232, 0, "Täby" },
                    { 233, 0, "Töreboda" },
                    { 234, 0, "Uddevalla" },
                    { 235, 0, "Ulricehamn" },
                    { 236, 0, "Umeå" },
                    { 237, 0, "Upplands-Bro" },
                    { 238, 0, "Upplands Väsby" },
                    { 239, 0, "Uppsala" },
                    { 240, 0, "Uppvidinge" },
                    { 241, 0, "Vadstena" },
                    { 242, 0, "Vaggeryd" },
                    { 243, 0, "Valdemarsvik" },
                    { 244, 0, "Vallentuna" },
                    { 245, 0, "Vansbro" },
                    { 246, 0, "Vara" },
                    { 247, 0, "Varberg" },
                    { 248, 0, "Vaxholm" },
                    { 249, 0, "Vellinge" },
                    { 250, 0, "Vetlanda" },
                    { 251, 0, "Vilhelmina" },
                    { 252, 0, "Vimmerby" },
                    { 253, 0, "Vindeln" },
                    { 254, 0, "Vingåker" },
                    { 255, 0, "Vårgårda" },
                    { 256, 0, "Vänersborg" },
                    { 257, 0, "Vännäs" },
                    { 258, 0, "Värmdö" },
                    { 259, 0, "Värnamo" },
                    { 260, 0, "Västervik" },
                    { 261, 0, "Västerås" },
                    { 262, 0, "Växjö" },
                    { 263, 0, "Ydre" },
                    { 264, 0, "Ystad" },
                    { 265, 0, "Åmål" },
                    { 266, 0, "Ånge" },
                    { 267, 0, "Åre" },
                    { 268, 0, "Årjäng" },
                    { 269, 0, "Åsele" },
                    { 270, 0, "Åstorp" },
                    { 271, 0, "Åtvidaberg" },
                    { 272, 0, "Älmhult" },
                    { 273, 0, "Älvdalen" },
                    { 274, 0, "Älvkarleby" },
                    { 275, 0, "Älvsbyn" },
                    { 276, 0, "Ängelholm" },
                    { 277, 0, "Öckerö" },
                    { 278, 0, "Ödeshög" },
                    { 279, 0, "Örebro" },
                    { 280, 0, "Örkelljunga" },
                    { 281, 0, "Örnsköldsvik" },
                    { 282, 0, "Östersund" },
                    { 283, 0, "Österåker" },
                    { 284, 0, "Östhammar" },
                    { 285, 0, "Östra Göinge" },
                    { 286, 0, "Överkalix" },
                    { 287, 0, "Övertorneå" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CouncilSeatAllocations_ElectionId",
                table: "CouncilSeatAllocations",
                column: "ElectionId");

            migrationBuilder.CreateIndex(
                name: "IX_CouncilSeatAllocations_ElectionResultId",
                table: "CouncilSeatAllocations",
                column: "ElectionResultId");

            migrationBuilder.CreateIndex(
                name: "IX_CouncilSeatAllocations_MunicipalityId_ElectionId_SeatNumber",
                table: "CouncilSeatAllocations",
                columns: new[] { "MunicipalityId", "ElectionId", "SeatNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CouncilSeatAllocationsScenarios_ElectionId",
                table: "CouncilSeatAllocationsScenarios",
                column: "ElectionId");

            migrationBuilder.CreateIndex(
                name: "IX_CouncilSeatAllocationsScenarios_MunicipalityId_ElectionId_SeatNumber_UserId",
                table: "CouncilSeatAllocationsScenarios",
                columns: new[] { "MunicipalityId", "ElectionId", "SeatNumber", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CouncilSeatAllocationsScenarios_PoliticalPartyId",
                table: "CouncilSeatAllocationsScenarios",
                column: "PoliticalPartyId");

            migrationBuilder.CreateIndex(
                name: "IX_CouncilSeatAllocationsScenarios_UserId",
                table: "CouncilSeatAllocationsScenarios",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ElectionResults_ElectionId",
                table: "ElectionResults",
                column: "ElectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ElectionResults_MunicipalityId_PoliticalPartyId_ElectionId",
                table: "ElectionResults",
                columns: new[] { "MunicipalityId", "PoliticalPartyId", "ElectionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ElectionResults_PoliticalPartyId",
                table: "ElectionResults",
                column: "PoliticalPartyId");

            migrationBuilder.CreateIndex(
                name: "IX_Elections_ElectionDate",
                table: "Elections",
                column: "ElectionDate",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Municipalities_Name",
                table: "Municipalities",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MunicipalityPoliticalParty_PoliticalPartiesId",
                table: "MunicipalityPoliticalParty",
                column: "PoliticalPartiesId");

            migrationBuilder.CreateIndex(
                name: "IX_PoliticalParties_PartyName",
                table: "PoliticalParties",
                column: "PartyName",
                unique: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CouncilSeatAllocations");

            migrationBuilder.DropTable(
                name: "CouncilSeatAllocationsScenarios");

            migrationBuilder.DropTable(
                name: "MunicipalityPoliticalParty");

            migrationBuilder.DropTable(
                name: "ElectionResults");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Elections");

            migrationBuilder.DropTable(
                name: "Municipalities");

            migrationBuilder.DropTable(
                name: "PoliticalParties");
        }
    }
}
