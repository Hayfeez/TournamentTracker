using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

using Newtonsoft.Json;

namespace TournamentTracker.Infrastructure.BasicResults
{
    public interface ICustomActionResult
    {
        [JsonIgnore]
        HttpStatusCode Status { get; set; }

        [JsonIgnore]
        string ErrorMessage { get; set; }
    }
}
