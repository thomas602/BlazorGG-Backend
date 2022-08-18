using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorGG_Backend.Migrations
{
    public partial class AddMoreColumnsIntoPlayers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileIconUrl",
                table: "Players",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TotalGames",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Winrate",
                table: "Players",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileIconUrl",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "TotalGames",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Winrate",
                table: "Players");
        }
    }
}
