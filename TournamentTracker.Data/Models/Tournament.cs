using System;
using System.Collections.Generic;

using TournamentTracker.Data.Interfaces;

namespace TournamentTracker.Data.Models
{
    public class Tournament : IAccount, IEntity, IAuditable, IDeletable
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public bool LowestScoreWins { get; set; }

        public string Name { get; set; }

        public decimal EntryFee { get; set; }

        public bool HasGroupStage { get; set; }
        public int TeamPerGroup { get; set; }



        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
