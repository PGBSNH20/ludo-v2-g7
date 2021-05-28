using Microsoft.EntityFrameworkCore.Migrations;

namespace LudoV2Api.Migrations
{
    public partial class addedThingsThatWasRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlayerGameGameId",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlayerGamePlayerId",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlayerGameGameId",
                table: "Games",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlayerGamePlayerId",
                table: "Games",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_PlayerGameGameId_PlayerGamePlayerId",
                table: "Players",
                columns: new[] { "PlayerGameGameId", "PlayerGamePlayerId" });

            migrationBuilder.CreateIndex(
                name: "IX_Games_PlayerGameGameId_PlayerGamePlayerId",
                table: "Games",
                columns: new[] { "PlayerGameGameId", "PlayerGamePlayerId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Games_GamePlayers_PlayerGameGameId_PlayerGamePlayerId",
                table: "Games",
                columns: new[] { "PlayerGameGameId", "PlayerGamePlayerId" },
                principalTable: "GamePlayers",
                principalColumns: new[] { "GameId", "PlayerId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_GamePlayers_PlayerGameGameId_PlayerGamePlayerId",
                table: "Players",
                columns: new[] { "PlayerGameGameId", "PlayerGamePlayerId" },
                principalTable: "GamePlayers",
                principalColumns: new[] { "GameId", "PlayerId" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_GamePlayers_PlayerGameGameId_PlayerGamePlayerId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_GamePlayers_PlayerGameGameId_PlayerGamePlayerId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_PlayerGameGameId_PlayerGamePlayerId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Games_PlayerGameGameId_PlayerGamePlayerId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "PlayerGameGameId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "PlayerGamePlayerId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "PlayerGameGameId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "PlayerGamePlayerId",
                table: "Games");
        }
    }
}
