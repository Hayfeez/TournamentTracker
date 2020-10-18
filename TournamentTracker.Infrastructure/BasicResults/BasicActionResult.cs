using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace TournamentTracker.Infrastructure.BasicResults
{
    public class BasicActionResult : ICustomActionResult
    {
        public HttpStatusCode Status { get; set; }

        public string ErrorMessage { get; set; }

        public BasicActionResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
            Status = HttpStatusCode.BadRequest;
        }

        public BasicActionResult()
        {
            Status = HttpStatusCode.OK;
        }

        public BasicActionResult(HttpStatusCode status)
        {
            Status = status;
        }
    }

  
}
