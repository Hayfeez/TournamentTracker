using System;
using System.Collections.Generic;
using System.Text;

using TournamentTracker.Common.Attributes;
using TournamentTracker.Data.Attributes;

namespace TournamentTracker.Data.Interfaces
{
    public interface IDeletable
    {
        [NotAutoMapped]
        bool IsDeleted { get; set; }

        [NotAutoMapped]
        [AuditableModificationDate]
        public DateTime? DeletedOn { get; set; }
    }
}
