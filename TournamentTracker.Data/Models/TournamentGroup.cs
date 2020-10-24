using System;
using System.Collections.Generic;

using TournamentTracker.Data.Interfaces;

namespace TournamentTracker.Data.Models
{
    public class TournamentGroup : IEntity, IAccount
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public Guid TournamentId { get; set; }
        public Guid GroupId { get; set; }

    }
}
