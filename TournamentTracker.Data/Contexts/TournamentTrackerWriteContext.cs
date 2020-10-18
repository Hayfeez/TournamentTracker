using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;

namespace TournamentTracker.Data.Contexts
{
    public class TournamentTrackerWriteContext : TournamentTrackerContext
    {
        public TournamentTrackerWriteContext(DbContextOptions<TournamentTrackerWriteContext> options) : base(options)
        {

        }

    }
}
