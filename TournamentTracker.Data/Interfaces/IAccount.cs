using System;
using System.Collections.Generic;
using System.Text;

using TournamentTracker.Common.Attributes;

namespace TournamentTracker.Data.Interfaces
{
    public interface IAccount
    {
        [RequireNonDefault]
        Guid AccountId { get; set; }
    }
}
