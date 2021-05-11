using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LudoV2Api.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GamePlayers",
                columns: table => new
                {
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePlayers", x => new { x.GameId, x.PlayerId });
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrentTurn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstPlace = table.Column<int>(type: "int", nullable: true),
                    SecondPlace = table.Column<int>(type: "int", nullable: true),
                    ThirdPlace = table.Column<int>(type: "int", nullable: true),
                    FourthPlace = table.Column<int>(type: "int", nullable: true),
                    LastSaved = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlayerGameGameId = table.Column<int>(type: "int", nullable: true),
                    PlayerGamePlayerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_GamePlayers_PlayerGameGameId_PlayerGamePlayerId",
                        columns: x => new { x.PlayerGameGameId, x.PlayerGamePlayerId },
                        principalTable: "GamePlayers",
                        principalColumns: new[] { "GameId", "PlayerId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlayerGameGameId = table.Column<int>(type: "int", nullable: true),
                    PlayerGamePlayerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_GamePlayers_PlayerGameGameId_PlayerGamePlayerId",
                        columns: x => new { x.PlayerGameGameId, x.PlayerGamePlayerId },
                        principalTable: "GamePlayers",
                        principalColumns: new[] { "GameId", "PlayerId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PawnSavePoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameId = table.Column<int>(type: "int", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlayerType = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PawnSavePoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PawnSavePoints_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_PlayerGameGameId_PlayerGamePlayerId",
                table: "Games",
                columns: new[] { "PlayerGameGameId", "PlayerGamePlayerId" });

            migrationBuilder.CreateIndex(
                name: "IX_PawnSavePoints_GameId",
                table: "PawnSavePoints",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_PlayerGameGameId_PlayerGamePlayerId",
                table: "Players",
                columns: new[] { "PlayerGameGameId", "PlayerGamePlayerId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PawnSavePoints");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "GamePlayers");
        }
    }
}
