using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace TournamentTracker.Api.Filters
{
    public class AccountScopeFilter : ActionFilterAttribute
    {
        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            const string xAccountId = "AccountId";

            var controller = actionContext.ControllerContext.Controller as ICustomControllerBase;
            var accountIdHeader = actionContext.Request.Headers
                .Where(x => x.Key.Equals(xAccountId, StringComparison.OrdinalIgnoreCase)) //Case sensitivity is a pain
                .SelectMany(x => x.Value)
                .FirstOrDefault();

            if (controller != null && accountIdHeader != null && Guid.TryParse(accountIdHeader, out var accountId))
            {
                controller.AccountId = accountId;
            }

            return base.OnActionExecutingAsync(actionContext, cancellationToken);
        }

    }
}
