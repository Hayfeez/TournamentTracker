using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using TournamentTracker.Api.ErrorLogger;
using TournamentTracker.Api.Filters;
using TournamentTracker.Infrastructure.Commands;
using TournamentTracker.Infrastructure.Commands.Tournaments;
using TournamentTracker.Infrastructure.Queries.Tournaments;

namespace TournamentTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TournamentController : CustomControllerBase
    {
        public TournamentController(IMediator mediator, ILoggerManager logger): base (mediator, logger)
        {
        }

        [HttpGet("")]
        [ValidateModel]
        [Produces("application/json")]
        public async Task<IActionResult> GetTournaments([FromQuery] GetTournaments.Query query)
        {
            query.AccountId = AccountId.GetValueOrDefault();

            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ValidateModel]
        [Produces("application/json")]
        public async Task<IActionResult> GetTournament([FromQuery] GetTournamentById.Query query, Guid id)
        {
            query.Id = id;
            query.AccountId = AccountId.GetValueOrDefault();

            var result = await Mediator.Send(query);
            return Ok(result);
        }


        [HttpPost("")]
        [ValidateModel]
        [Produces("application/json")]
        public async Task<IActionResult> AddTournament([FromBody] CreateTournament.Request request)
        {
            request.AccountId = AccountId.GetValueOrDefault();
            request.ActionBy = UserId.GetValueOrDefault();

            var result = await Mediator.Send(request);
            return Respond(result);
        }

        [HttpPut("{id}")]
        [ValidateModel]
        [Produces("application/json")]
        public async Task<IActionResult> UpdateTournament([FromBody] UpdateTournament.Request request, Guid id)
        {
            request.Id = id;
            request.AccountId = AccountId.GetValueOrDefault();
            request.ActionBy = UserId.GetValueOrDefault();

            var result = await Mediator.Send(request);
            return Respond(result);
        }

        [HttpDelete("{id}")]
        [ValidateModel]
        [Produces("application/json")]
        public async Task<IActionResult> DeleteTournament([FromBody] DeleteCommand.Request request, Guid id)
        {
            request.Id = id;
            request.AccountId = AccountId.GetValueOrDefault();
            request.ActionBy = UserId.GetValueOrDefault();

            var result = await Mediator.Send(request);
            return Respond(result);
        }
    }
}
