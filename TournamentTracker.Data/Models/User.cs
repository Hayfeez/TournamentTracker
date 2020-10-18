using System;

using TournamentTracker.Data.Interfaces;

namespace TournamentTracker.Data.Models
{
    public class User : IEntity, IAuditable, IDeletable
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
