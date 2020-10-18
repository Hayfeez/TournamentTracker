using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

using TournamentTracker.Data.Attributes;

namespace TournamentTracker.Data.Interfaces
{
    public interface IEntity
    {
        [Key]
        [GenerateSequentialGuid]
        Guid Id { get; set; }
    }
}
