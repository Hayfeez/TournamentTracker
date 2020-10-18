using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace TournamentTracker.Data.Contexts
{
    public class TournamentTrackerReadContext : TournamentTrackerContext
    {
        public TournamentTrackerReadContext(DbContextOptions<TournamentTrackerReadContext> options) : base(options)
        {
                
        }

        public override int SaveChanges()
        {
            //return base.SaveChanges();
            throw new System.Exception($"Cannot save using the {nameof(TournamentTrackerReadContext)}. No writes can be used with this context.");
        }

        //[Obsolete("Cannot use the save changes on the read context.", true)]
        //public override Task<int> SaveChangesAsync()
        //{
        //    throw new Exception($"Cannot save using the {nameof(TournamentTrackerReadContext)}. No writes can be used with this context.");
        //}

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            throw new Exception($"Cannot save using the {nameof(TournamentTrackerReadContext)}. No writes can be used with this context.");
        }
    }
}
