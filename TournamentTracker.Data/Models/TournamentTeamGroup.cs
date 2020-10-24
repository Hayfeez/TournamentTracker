using System;
using System.Collections.Generic;

using TournamentTracker.Data.Interfaces;

namespace TournamentTracker.Data.Models
{
    public class TournamentTeamGroup : IEntity, IAccount
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public Guid TournamentId { get; set; }
        public Guid TournamentTeamId { get; set; }

        public Guid TournamentGroupId { get; set; }

    }
}
