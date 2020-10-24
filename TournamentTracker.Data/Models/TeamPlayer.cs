using System;

using TournamentTracker.Common.Enums;
using TournamentTracker.Data.Interfaces;

namespace TournamentTracker.Data.Models
{
    public class TeamPlayer : IEntity, IAccount, IAuditable, IDeletable
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid TeamId { get; set; }
        public Guid PlayerId { get; set; }

        public bool IsCaptain { get; set; } = false;
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

    }
}
