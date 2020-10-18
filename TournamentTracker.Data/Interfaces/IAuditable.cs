using System;
using System.Collections.Generic;
using System.Text;

using TournamentTracker.Common.Attributes;
using TournamentTracker.Data.Attributes;

namespace TournamentTracker.Data.Interfaces
{
    public interface IAuditable
    {
        [NotAutoMapped]
        [AuditableModificationDate]
        public DateTime? CreatedOn { get; set; }

        [NotAutoMapped]
        [AuditableModificationDate]
        public DateTime? ModifiedOn { get; set; }

    }
}
