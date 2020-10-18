using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TournamentTracker.Data.Migrations
{
    public partial class DeletePrizeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prize");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "User");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PrizeId",
                table: "TournamentPrize");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Player");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "TournamentPrize",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsPercentage",
                table: "TournamentPrize",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "TournamentPrize",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rank",
                table: "TournamentPrize",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PlayerNo",
                table: "Player",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "TournamentPrize");

            migrationBuilder.DropColumn(
                name: "IsPercentage",
                table: "TournamentPrize");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "TournamentPrize");

            migrationBuilder.DropColumn(
                name: "Rank",
                table: "TournamentPrize");

            migrationBuilder.DropColumn(
                name: "PlayerNo",
                table: "Player");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "User",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "PrizeId",
                table: "TournamentPrize",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Player",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Prize",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsPercentage = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rank = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prize", x => x.Id);
                });
        }
    }
}
