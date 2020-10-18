using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TournamentTracker.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fixture",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RoundId = table.Column<Guid>(nullable: false),
                    Winner = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fixture", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FixtureEntry",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FixtureId = table.Column<Guid>(nullable: false),
                    TeamId = table.Column<Guid>(nullable: false),
                    Score = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixtureEntry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Prize",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Rank = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    IsPercentage = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prize", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    TeamCaptain = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeamPlayer",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TeamId = table.Column<Guid>(nullable: false),
                    PlayerId = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamPlayer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tournament",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    LowestScoreWins = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    EntryFee = table.Column<decimal>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournament", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TournamentPrize",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TournamentId = table.Column<Guid>(nullable: false),
                    PrizeId = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentPrize", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TournamentRound",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TournamentId = table.Column<Guid>(nullable: false),
                    Round = table.Column<string>(nullable: true),
                    RoundRank = table.Column<int>(nullable: false),
                    NoOfTeams = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentRound", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TournamentStat",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TournamentId = table.Column<Guid>(nullable: false),
                    FixtureId = table.Column<Guid>(nullable: false),
                    TeamId = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Value = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentStat", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TournamentTeam",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TournamentId = table.Column<Guid>(nullable: false),
                    TeamId = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentTeam", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fixture");

            migrationBuilder.DropTable(
                name: "FixtureEntry");

            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.DropTable(
                name: "Prize");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "TeamPlayer");

            migrationBuilder.DropTable(
                name: "Tournament");

            migrationBuilder.DropTable(
                name: "TournamentPrize");

            migrationBuilder.DropTable(
                name: "TournamentRound");

            migrationBuilder.DropTable(
                name: "TournamentStat");

            migrationBuilder.DropTable(
                name: "TournamentTeam");
        }
    }
}
