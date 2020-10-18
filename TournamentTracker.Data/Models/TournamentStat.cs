using System;

using TournamentTracker.Common.Enums;
using TournamentTracker.Data.Interfaces;

namespace TournamentTracker.Data.Models
{
    public class TournamentStat : IEntity, IAccount, IDeletable
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid TournamentId { get; set; }
        public Guid FixtureId { get; set; }
        public Guid TeamId { get; set; }
        public StatType Type { get; set; }
        public int Value { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }

    }
}
