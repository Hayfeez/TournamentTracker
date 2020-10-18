using System;
using System.Collections.Generic;
using System.Text;

namespace TournamentTracker.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class NotAutoMappedAttribute : Attribute
    {
    }

}
