using Microsoft.EntityFrameworkCore.Migrations;

namespace LudoV2Api.Migrations
{
    public partial class AddedColorToPlayerGameTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "GamePlayers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "GamePlayers");
        }
    }
}
