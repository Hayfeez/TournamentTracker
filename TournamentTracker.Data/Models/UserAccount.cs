using System;
using System.Collections.Generic;

using TournamentTracker.Data.Interfaces;

namespace TournamentTracker.Data.Models
{
    public class UserAccount : IEntity, IAccount, IAuditable, IDeletable
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public Guid UserId { get; set; }

        public bool IsPending { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
