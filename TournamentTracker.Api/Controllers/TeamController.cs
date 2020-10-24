using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using TournamentTracker.Api.ErrorLogger;
using TournamentTracker.Api.Filters;
using TournamentTracker.Infrastructure.Commands.Teams;
using TournamentTracker.Infrastructure.Queries.Teams;


namespace TournamentTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TeamController : CustomControllerBase
    {
        public TeamController(IMediator mediator, ILoggerManager logger): base (mediator, logger)
        {
        }

        [HttpGet("")]
        [ValidateModel]
        public async Task<IActionResult> GetTeams()
        {
            var query = new GetTeams.Query
            {
                AccountId = AccountId.GetValueOrDefault()
            };

            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ValidateModel]
        public async Task<IActionResult> GetTeam(Guid id)
        {
            var query = new GetTeamById.Query
            {
                Id = id,
                AccountId = AccountId.GetValueOrDefault()
            };

            var result = await Mediator.Send(query);
            return Ok(result);
        }


        [HttpPost("")]
        [ValidateModel]
        public async Task<IActionResult> AddTeam([FromBody] CreateTeam.Request request)
        {
            request.AccountId = AccountId.GetValueOrDefault();
            request.ActionBy = UserId.GetValueOrDefault();

            var result = await Mediator.Send(request);
            return Respond(result);
        }

        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateTeam([FromBody] UpdateTeam.Request request, Guid id)
        {
            request.Id = id;
            request.AccountId = AccountId.GetValueOrDefault();
            request.ActionBy = UserId.GetValueOrDefault();

            var result = await Mediator.Send(request);
            return Respond(result);
        }

        [HttpDelete("{id}")]
        [ValidateModel]
        public async Task<IActionResult> DeleteTeam(Guid id)
        {
            var request = new DeleteTeam.Request
            {
                Id = id,
                AccountId = AccountId.GetValueOrDefault(),
                ActionBy = UserId.GetValueOrDefault(),
            };

            var result = await Mediator.Send(request);
            return Respond(result);
        }
    }
}
