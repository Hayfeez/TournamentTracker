using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TournamentTracker.Data.Contexts
{
    public class ReadWriteContextFactory : IDesignTimeDbContextFactory<TournamentTrackerWriteContext>
    {
        public TournamentTrackerWriteContext CreateDbContext(string[] args)
        {
            // Get environment
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // Build config
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            // Get connection string
            var optionsBuilder = new DbContextOptionsBuilder<TournamentTrackerWriteContext>();
            var connectionString = config.GetConnectionString("TournamentTrackerWriteContext");
            optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("TournamentTracker.Data"));
            return new TournamentTrackerWriteContext(optionsBuilder.Options);
        }
    }
}
