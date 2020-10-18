using System;
using System.Collections.Generic;

using TournamentTracker.Data.Interfaces;

namespace TournamentTracker.Data.Models
{
    public class Fixture : IEntity, IAccount
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid RoundId { get; set; }
        public Guid? Winner { get; set; }
    }
}
