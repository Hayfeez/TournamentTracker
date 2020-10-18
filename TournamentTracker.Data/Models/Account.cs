using System;
using System.Collections.Generic;

using TournamentTracker.Data.Interfaces;

namespace TournamentTracker.Data.Models
{
    public class Account : IEntity, IAuditable, IDeletable
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Domain { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
