using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using TournamentTracker.Api.ErrorLogger;
using TournamentTracker.Api.Filters;
using TournamentTracker.Infrastructure.Commands;
using TournamentTracker.Infrastructure.Commands.Accounts;
using TournamentTracker.Infrastructure.Queries.Teams;

namespace TournamentTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AccountController : CustomControllerBase
    {
        public AccountController(IMediator mediator, ILoggerManager logger): base (mediator, logger)
        {
        }

        [HttpGet("")]
        [ValidateModel]
        public async Task<IActionResult> GetAccounts([FromQuery] GetAccounts.Query query)
        {
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ValidateModel]
        public async Task<IActionResult> GetAccount([FromQuery] GetAccountById.Query query, Guid id)
        {
            query.Id = id;

            var result = await Mediator.Send(query);
            return Ok(result);
        }


        [HttpPost("")]
        [ValidateModel]
        public async Task<IActionResult> AddAccount([FromBody] CreateAccount.Request request)
        {
            request.UserId = UserId.GetValueOrDefault();

            var result = await Mediator.Send(request);
            return Respond(result);
        }

        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccount.Request request, Guid id)
        {
            request.Id = id;
            request.UserId = UserId.GetValueOrDefault();

            var result = await Mediator.Send(request);
            return Respond(result);
        }

        [HttpDelete("{id}")]
        [ValidateModel]
        public async Task<IActionResult> DeleteAccount([FromBody] DeleteCommand.Request request, Guid id)
        {
            request.Id = id;
            request.AccountId = id;

            var result = await Mediator.Send(request);
            return Respond(result);
        }
    }
}
