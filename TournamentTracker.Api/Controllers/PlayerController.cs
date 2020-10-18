using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using TournamentTracker.Api.ErrorLogger;
using TournamentTracker.Api.Filters;
using TournamentTracker.Infrastructure.Commands;
using TournamentTracker.Infrastructure.Commands.Players;
using TournamentTracker.Infrastructure.Queries.Players;

namespace TournamentTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PlayerController : CustomControllerBase
    {
        public PlayerController(IMediator mediator, ILoggerManager logger): base (mediator, logger)
        {
        }

        [HttpGet("")]
        [ValidateModel]
        public async Task<IActionResult> GetPlayers([FromQuery] GetPlayers.Query query)
        {
            query.AccountId = AccountId.GetValueOrDefault();
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ValidateModel]
        public async Task<IActionResult> GetPlayer([FromQuery] GetPlayerById.Query query, Guid id)
        {
            query.AccountId = AccountId.GetValueOrDefault();
            query.Id = id;

            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("in-team")]
        [ValidateModel]
        public async Task<IActionResult> GetPlayersInTeam([FromQuery] GetPlayersInTeam.Query query, [FromQuery]Guid teamId)
        {
            query.AccountId = AccountId.GetValueOrDefault();
            query.TeamId = teamId;

            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("not-in-team")]
        [ValidateModel]
        public async Task<IActionResult> GetPlayersNotInTeam([FromQuery] GetPlayersNotInTeam.Query query, [FromQuery]Guid teamId)
        {
            query.AccountId = AccountId.GetValueOrDefault();
            query.TeamId = teamId;

            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{playerId}/teams")]
        [ValidateModel]
        public async Task<IActionResult> GetPlayersTeams([FromQuery] GetPlayersTeams.Query query, Guid playerId)
        {
            query.AccountId = AccountId.GetValueOrDefault();
            query.PlayerId = playerId;

            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("")]
        [ValidateModel]
        public async Task<IActionResult> AddAccount([FromBody] CreatePlayer.Request request)
        {
            request.AccountId = AccountId.GetValueOrDefault();
            request.ActionBy = UserId.GetValueOrDefault();

            var result = await Mediator.Send(request);
            return Respond(result);
        }

        [HttpPost("assign/{teamId}")]
        [ValidateModel]
        public async Task<IActionResult> AssignPlayersToTeam([FromBody] AssignPlayersToTeam.Request request, Guid teamId)
        {
            request.AccountId = AccountId.GetValueOrDefault();
            request.ActionBy = UserId.GetValueOrDefault();
            request.TeamId = teamId;

            var result = await Mediator.Send(request);
            return Respond(result);
        }


        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> UpdatePlayer([FromBody] UpdatePlayer.Request request, Guid id)
        {
            request.Id = id;
            request.ActionBy = UserId.GetValueOrDefault();
            request.AccountId = AccountId.GetValueOrDefault();

            var result = await Mediator.Send(request);
            return Respond(result);
        }

        [HttpDelete("")]
        [ValidateModel]
        public async Task<IActionResult> RemovePlayerFromTeam([FromBody] RemovePlayerFromTeam.Request request)
        {
            request.AccountId = AccountId.GetValueOrDefault();
            request.ActionBy = UserId.GetValueOrDefault();

            var result = await Mediator.Send(request);
            return Respond(result);
        }

        [HttpDelete("{id}")]
        [ValidateModel]
        public async Task<IActionResult> DeletePlayer([FromBody] DeleteCommand.Request request, Guid id)
        {
            request.Id = id;
            request.AccountId = AccountId.GetValueOrDefault();
            request.ActionBy = UserId.GetValueOrDefault();

            var result = await Mediator.Send(request);
            return Respond(result);
        }
    }
}
