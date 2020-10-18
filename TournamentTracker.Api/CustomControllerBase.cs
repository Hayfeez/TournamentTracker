using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using TournamentTracker.Api.ErrorLogger;
using TournamentTracker.Infrastructure.BasicResults;

namespace TournamentTracker.Api
{
    public class CustomControllerBase : ControllerBase
    {
        public CustomControllerBase(IMediator mediator, ILoggerManager logger)
        {
            Mediator = mediator;
            Logger = logger;
        }

        public IMediator Mediator { get; }
        public ILoggerManager Logger { get; }

        public Guid? AccountId { get; set; }

        public Guid? UserId { get; set; }

        public CurrentUser CurrentUser { get; set; }

        [NonAction]
        public IActionResult Respond(ICustomActionResult result)
        {
            if (result.Status == HttpStatusCode.OK)
            {
                return Ok(result);
            }

            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                var objectResult = new ObjectResult(result.ErrorMessage)
                {
                    StatusCode = (int)result.Status
                };

                return objectResult;
            }

            return StatusCode((int)result.Status);
        }
    }
}
