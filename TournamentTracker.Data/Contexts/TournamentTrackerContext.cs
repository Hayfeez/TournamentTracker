using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

using Microsoft.EntityFrameworkCore;

using TournamentTracker.Data.Models;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace TournamentTracker.Data.Contexts
{
    public class TournamentTrackerContext : DbContext
    {
        public TournamentTrackerContext(DbContextOptions<TournamentTrackerReadContext> options) : base(options)
        {
        }

        public TournamentTrackerContext(DbContextOptions<TournamentTrackerWriteContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Fixture> Fixtures { get; set; }

        public DbSet<FixtureEntry> FixtureEntries { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<TeamPlayer> TeamPlayers { get; set; }

        public DbSet<Tournament> Tournaments { get; set; }

        public DbSet<Group> Groups { get; set; }
        public DbSet<TournamentGroup> TournamentGroups { get; set; }
        public DbSet<TournamentTeam> TournamentTeams { get; set; }
        public DbSet<TournamentTeamGroup> TeamGroups { get; set; }

        public DbSet<TournamentPrize> TournamentPrizes { get; set; }

        public DbSet<FixtureStat> TournamentStats { get; set; }

        public DbSet<TournamentRound> TournamentRounds { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=192.168.0.137;Initial Catalog=TournamentTrackerDb;User Id=tournamentLogin;Password=Password@123");

            // Get environment
            //var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            //// Build config
            //IConfiguration config = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //    .AddJsonFile($"appsettings.{environment}.json", optional: true)
            //    .AddEnvironmentVariables()
            //    .Build();

            //// Get connection string
            //var connectionString = config.GetConnectionString("TournamentTrackerReadWriteContext");

         //   optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("TournamentTracker.Data"));
         //   optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Account>().ToTable("Account");
            modelBuilder.Entity<Group>().ToTable("Group");
            modelBuilder.Entity<Player>().ToTable("Player");
            modelBuilder.Entity<Team>().ToTable("Team");
            modelBuilder.Entity<Tournament>().ToTable("Tournament");

            modelBuilder.Entity<TeamPlayer>().ToTable("TeamPlayer");
            modelBuilder.Entity<UserAccount>().ToTable("UserAccount");
            modelBuilder.Entity<TournamentGroup>().ToTable("TournamentGroup");
            modelBuilder.Entity<TournamentTeam>().ToTable("TournamentTeam");
            modelBuilder.Entity<TournamentPrize>().ToTable("TournamentPrize");

            modelBuilder.Entity<TournamentTeamGroup>().ToTable("TournamentTeamGroup");
            modelBuilder.Entity<TournamentRound>().ToTable("TournamentRound");
            modelBuilder.Entity<Fixture>().ToTable("Fixture");
            modelBuilder.Entity<FixtureEntry>().ToTable("FixtureEntry");
            modelBuilder.Entity<FixtureStat>().ToTable("FixtureStat");
        }
    }
}
