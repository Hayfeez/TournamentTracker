using System;

using TournamentTracker.Data.Interfaces;

namespace TournamentTracker.Data.Models
{
    public class FixtureEntry : IEntity, IAccount
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid FixtureId { get; set; }
        public Guid TeamId { get; set; }
        public double Score { get; set; }
    }
}
