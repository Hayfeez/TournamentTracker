using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

using Microsoft.EntityFrameworkCore;

using TournamentTracker.Data.Models;

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

        public DbSet<Prize> Prizes { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<TeamPlayer> TeamPlayers { get; set; }

        public DbSet<Tournament> Tournaments { get; set; }

        public DbSet<TournamentTeam> TournamentTeams { get; set; }

        public DbSet<TournamentPrize> TournamentPrizes { get; set; }

        public DbSet<TournamentStat> TournamentStats { get; set; }

        public DbSet<TournamentRound> TournamentRounds { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=192.168.0.137;Initial Catalog=TournamentTrackerDb;User Id=tournamentLogin;Password=Password@123");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().ToTable("Account");
            modelBuilder.Entity<Fixture>().ToTable("Fixture");
            modelBuilder.Entity<FixtureEntry>().ToTable("FixtureEntry");
            modelBuilder.Entity<Player>().ToTable("Player");
            modelBuilder.Entity<Prize>().ToTable("Prize");
            modelBuilder.Entity<Team>().ToTable("Team");
            modelBuilder.Entity<TeamPlayer>().ToTable("TeamPlayer");
            modelBuilder.Entity<Tournament>().ToTable("Tournament");
            modelBuilder.Entity<TournamentRound>().ToTable("TournamentRound");
            modelBuilder.Entity<TournamentTeam>().ToTable("TournamentTeam");
            modelBuilder.Entity<TournamentPrize>().ToTable("TournamentPrize");
            modelBuilder.Entity<TournamentStat>().ToTable("TournamentStat");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<UserAccount>().ToTable("UserAccount");
        }
    }
}
