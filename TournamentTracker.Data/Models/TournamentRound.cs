using System;

using TournamentTracker.Common.Enums;
using TournamentTracker.Data.Interfaces;

namespace TournamentTracker.Data.Models
{
    public class TournamentRound : IEntity, IAccount
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid TournamentId { get; set; }
        public string Round { get; set; }
        public int RoundRank { get; set; }
        public int TeamsInRound { get; set; }
    }
}
