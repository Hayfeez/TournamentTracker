using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using TournamentTracker.Api.ErrorLogger;
using TournamentTracker.Api.Filters;
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
        public async Task<IActionResult> GetPlayers()
        {
            var query = new GetPlayers.Query
            {
                AccountId = AccountId.GetValueOrDefault()
            };
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ValidateModel]
        public async Task<IActionResult> GetPlayer(Guid id)
        {
            var query = new GetPlayerById.Query
            {
                AccountId = AccountId.GetValueOrDefault(),
                Id = id
            };

            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("in-team/{teamId}")]
        [ValidateModel]
        public async Task<IActionResult> GetPlayersInTeam(Guid teamId)
        {
            var query = new GetPlayersInTeam.Query
            {
                AccountId = AccountId.GetValueOrDefault(),
                TeamId = teamId
            };

            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("not-in-team/{teamId}")]
        [ValidateModel]
        public async Task<IActionResult> GetPlayersNotInTeam(Guid teamId)
        {
            var query = new GetPlayersNotInTeam.Query
            {
                AccountId = AccountId.GetValueOrDefault(),
                TeamId = teamId
            };

            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{playerId}/teams")]
        [ValidateModel]
        public async Task<IActionResult> GetPlayersTeams(Guid playerId)
        {
            var query = new GetPlayersTeams.Query
            {
                AccountId = AccountId.GetValueOrDefault(),
                PlayerId = playerId
            };

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

        [HttpDelete("{playerId}/remove/{teamId}")]
        [ValidateModel]
        public async Task<IActionResult> RemovePlayerFromTeam(Guid teamId, Guid playerId)
        {
            var request = new RemovePlayerFromTeam.Request
            {
                AccountId = AccountId.GetValueOrDefault(),
                ActionBy = UserId.GetValueOrDefault(),
                TeamId = teamId,
                PlayerId = playerId
            };

            var result = await Mediator.Send(request);
            return Respond(result);
        }

        [HttpDelete("{id}")]
        [ValidateModel]
        public async Task<IActionResult> DeletePlayer(Guid id)
        {
            var request = new DeletePlayer.Request
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
