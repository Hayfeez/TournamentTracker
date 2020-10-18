using System;

using TournamentTracker.Common.Enums;
using TournamentTracker.Data.Interfaces;

namespace TournamentTracker.Data.Models
{
    public class TournamentPrize : IEntity, IAccount, IAuditable, IDeletable
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid TournamentId { get; set; }
        public Guid PrizeId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
