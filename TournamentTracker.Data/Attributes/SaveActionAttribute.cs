using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace TournamentTracker.Data.Attributes
{
    /// <summary>
    /// Base attribute for SaveAction Attributes
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class SaveActionAttribute : Attribute
    {
        public abstract bool CanPerform(Type type);

        public abstract object Perform([CanBeNull] object value, EntityState state, [CanBeNull] object originalValue, object parentEntity, string propertyName);
    }
}
