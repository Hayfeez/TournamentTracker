using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;

namespace TournamentTracker.Data.Attributes
{
    /// <summary>
    /// Sets modification date
    /// </summary>
    public class AuditableModificationDateAttribute : SaveActionAttribute
    {
        public override bool CanPerform(Type type)
        {
            return type == typeof(DateTime?);
        }

        public override object Perform(object value, EntityState state, object originalValue, object parentEntity, string propertyName)
        {
            var shouldAct = state == EntityState.Modified || state == EntityState.Added;
            return shouldAct ? DateTime.UtcNow : value;
        }
    }
}
