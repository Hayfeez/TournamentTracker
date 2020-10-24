using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;

using TournamentTracker.Infrastructure.Queries.Users;

namespace TournamentTracker.Api.Filters
{
    public class UserAuthorizationFilter : AuthorizationFilterAttribute
    {
        [Inject]
        public IConfiguration Configuration { get; set; }

        [Inject]
        public IMediator Mediator { get; set; }


        public override async Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            if (actionContext.ControllerContext.Controller is CustomControllerBase controller)
            {
                if (controller.UserId.HasValue)
                {
                    await base.OnAuthorizationAsync(actionContext, cancellationToken);
                    return;
                }

                if (await HandleVerificationWithUserId(actionContext, controller))
                {
                    await base.OnAuthorizationAsync(actionContext, cancellationToken);
                    return;
                }
            }

            actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized,
                "Invalid user credential or client is not authorized to use this endpoint. If this endpoint requires authentication then consider checking if token within the Authorization header is valid and has not expired.");

            await base.OnAuthorizationAsync(actionContext, cancellationToken);
        }


        private async Task<bool> HandleVerificationWithUserId(HttpActionContext actionContext, ICustomControllerBase controller)
        {
            var environment = Configuration["AppEnvironment"];

            //Only dev and local environments should allow the use of the login id as a valid authorization.
            if (environment != "Development" && environment != "Local")
            {
                return false;
            }

            var authHeader = actionContext.Request.Headers.Authorization;
            if (controller != null && authHeader?.Scheme == "loginId" && Guid.TryParse(authHeader.Parameter, out var loginId))
            {
                var result = await Mediator.Send(new GetUserById.Query { Id = loginId });
                if (result == null || result.Id == Guid.Empty)
                {
                    return false;
                }

                controller.UserId = result.Id;
                return true;
            }
            return false;
        }

    }
}
