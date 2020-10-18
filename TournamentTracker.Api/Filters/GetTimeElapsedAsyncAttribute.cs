using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TournamentTracker.Api.Filters
{
    public class GetTimeElapsedAsyncAttribute : ActionFilterAttribute
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Stopwatch timer = Stopwatch.StartNew();
            await next();
            timer.Stop();
            string result = "<div>Elapsed time: " + $"{timer.Elapsed.TotalMilliseconds} ms</div>";
            byte[] bytes = Encoding.ASCII.GetBytes(result);
            await context.HttpContext.Response.Body.WriteAsync(bytes, 0, bytes.Length);
        }

    }
}
