using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TournamentTracker.Api.Filters
{
    public class GetTimeElapsedAttribute : ActionFilterAttribute
    {
        private Stopwatch timer;
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            timer = Stopwatch.StartNew();
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            timer.Stop();
            string result = " Elapsed time: " + $"{timer.Elapsed.TotalMilliseconds} ms";
            IActionResult iActionResult = context.Result;
            ((ObjectResult)iActionResult).Value += Environment.NewLine + result;
        }

    }
}
