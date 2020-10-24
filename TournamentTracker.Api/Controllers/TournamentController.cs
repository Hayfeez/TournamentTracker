using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using TournamentTracker.Api.ErrorLogger;
using TournamentTracker.Api.Filters;
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

        #region tournament

        [HttpGet("")]
        [ValidateModel]
        [Produces("application/json")]
        public async Task<IActionResult> GetTournaments()
        {
            var query = new GetTournaments.Query
            {
                AccountId = AccountId.GetValueOrDefault()
            };

            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ValidateModel]
        [Produces("application/json")]
        public async Task<IActionResult> GetTournament(Guid id)
        {
            var query = new GetTournamentById.Query
            {
                Id = id,
                AccountId = AccountId.GetValueOrDefault()
            };

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
        public async Task<IActionResult> DeleteTournament(Guid id)
        {
            var request = new DeleteTournament.Request
            {
                Id = id,
                AccountId = AccountId.GetValueOrDefault(),
                ActionBy = UserId.GetValueOrDefault(),
            };

            var result = await Mediator.Send(request);
            return Respond(result);
        }

        #endregion

        #region tournament team

        [HttpGet("{tournamentId}/team")]
        [ValidateModel]
        [Produces("application/json")]
        public async Task<IActionResult> GetTournamentTeams(Guid tournamentId)
        {
            var query = new GetTournamentTeams.Query
            {
                AccountId = AccountId.GetValueOrDefault(),
                TournamentId = tournamentId
            };

            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{tournamentId}/team/{id}")]
        [ValidateModel]
        [Produces("application/json")]
        public async Task<IActionResult> GetTournamentTeam(Guid id, Guid tournamentId)
        {
            var query = new GetTournamentTeamById.Query
            {
                Id = id,
                TournamentId = tournamentId,
                AccountId = AccountId.GetValueOrDefault()
            };

            var result = await Mediator.Send(query);
            return Ok(result);
        }


        [HttpPost("{tournamentId}/team")]
        [ValidateModel]
        [Produces("application/json")]
        public async Task<IActionResult> AddTournamentTeam([FromBody] AddTournamentTeams.Request request, Guid tournamentId)
        {
            request.TournamentId = tournamentId;
            request.AccountId = AccountId.GetValueOrDefault();
            request.ActionBy = UserId.GetValueOrDefault();

            var result = await Mediator.Send(request);
            return Respond(result);
        }

        [HttpDelete("{tournamentId}/team/{id}")]
        [ValidateModel]
        [Produces("application/json")]
        public async Task<IActionResult> DeleteTournamentTeam(Guid tournamentId, Guid id)
        {
            var request = new DeleteTournamentTeam.Request
            {
                Id = id,
                TournamentId = tournamentId,
                AccountId = AccountId.GetValueOrDefault(),
                ActionBy = UserId.GetValueOrDefault()
            };

            var result = await Mediator.Send(request);
            return Respond(result);
        }

        #endregion

        #region tournament group

        [HttpGet("{tournamentId}/group")]
        [ValidateModel]
        [Produces("application/json")]
        public async Task<IActionResult> GetTournamentGroups(Guid tournamentId)
        {
            var query = new GetTournamentGroups.Query
            {
                AccountId = AccountId.GetValueOrDefault(),
                TournamentId = tournamentId
            };

            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{tournamentId}/group/{id}")]
        [ValidateModel]
        [Produces("application/json")]
        public async Task<IActionResult> GetTournamentGroup(Guid id, Guid tournamentId)
        {
            var query = new GetTournamentGroupById.Query
            {
                Id = id,
                TournamentId = tournamentId,
                AccountId = AccountId.GetValueOrDefault()
            };

            var result = await Mediator.Send(query);
            return Ok(result);
        }


        [HttpPost("{tournamentId}/group")]
        [ValidateModel]
        [Produces("application/json")]
        public async Task<IActionResult> AddTournamentGroup([FromBody] AddTournamentGroups.Request request, Guid tournamentId)
        {
            request.TournamentId = tournamentId;
            request.AccountId = AccountId.GetValueOrDefault();
            request.ActionBy = UserId.GetValueOrDefault();

            var result = await Mediator.Send(request);
            return Respond(result);
        }

        [HttpDelete("{tournamentId}/group/{id}")]
        [ValidateModel]
        [Produces("application/json")]
        public async Task<IActionResult> DeleteTournamentGroup(Guid tournamentId, Guid id)
        {
            var request = new DeleteTournamentGroup.Request
            {
                Id = id,
                TournamentId = tournamentId,
                AccountId = AccountId.GetValueOrDefault(),
                ActionBy = UserId.GetValueOrDefault()
            };

            var result = await Mediator.Send(request);
            return Respond(result);
        }

        #endregion

        #region tournament prize

        [HttpGet("{tournamentId}/prize")]
        [ValidateModel]
        [Produces("application/json")]
        public async Task<IActionResult> GetTournamentPrizes(Guid tournamentId)
        {
            var query = new GetTournamentPrizes.Query
            {
                AccountId = AccountId.GetValueOrDefault(),
                TournamentId = tournamentId
            };

            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{tournamentId}/prize/{id}")]
        [ValidateModel]
        [Produces("application/json")]
        public async Task<IActionResult> GetTournamentPrize(Guid id, Guid tournamentId)
        {
            var query = new GetTournamentPrizeById.Query
            {
                Id = id,
                TournamentId = tournamentId,
                AccountId = AccountId.GetValueOrDefault()
            };

            var result = await Mediator.Send(query);
            return Ok(result);
        }


        [HttpPost("{tournamentId}/prize")]
        [ValidateModel]
        [Produces("application/json")]
        public async Task<IActionResult> AddTournamentPrize([FromBody] CreateTournamentPrize.Request request, Guid tournamentId)
        {
            request.TournamentId = tournamentId;
            request.AccountId = AccountId.GetValueOrDefault();
            request.ActionBy = UserId.GetValueOrDefault();

            var result = await Mediator.Send(request);
            return Respond(result);
        }

        [HttpPut("{tournamentId}/prize/{prizeId}")]
        [ValidateModel]
        [Produces("application/json")]
        public async Task<IActionResult> UpdateTournamentPrize([FromBody] UpdateTournamentPrize.Request request, Guid tournamentId, Guid prizeId)
        {
            request.Id = prizeId;
            request.TournamentId = tournamentId;
            request.AccountId = AccountId.GetValueOrDefault();
            request.ActionBy = UserId.GetValueOrDefault();

            var result = await Mediator.Send(request);
            return Respond(result);
        }

        [HttpDelete("{tournamentId}/prize")]
        [ValidateModel]
        [Produces("application/json")]
        public async Task<IActionResult> DeleteTournamentPrizes(Guid tournamentId)
        {
            var request = new DeleteTournamentPrize.Request
            {
                TournamentId = tournamentId,
                AccountId = AccountId.GetValueOrDefault(),
                ActionBy = UserId.GetValueOrDefault()
            };

            var result = await Mediator.Send(request);
            return Respond(result);
        }

        #endregion

        #region tournament teamgroup

        [HttpGet("{tournamentId}/team-group")]
        [ValidateModel]
        [Produces("application/json")]
        public async Task<IActionResult> GetTournamentTeamGroups(Guid tournamentId)
        {
            var query = new GetTournamentTeamGroups.Query
            {
                AccountId = AccountId.GetValueOrDefault(),
                TournamentId = tournamentId
            };

            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("{tournamentId}/team-group")]
        [ValidateModel]
        [Produces("application/json")]
        public async Task<IActionResult> AddTournamentTeamGroups(Guid tournamentId)
        {
            var request = new AddTournamentTeamsToGroups.Request
            {
                TournamentId = tournamentId,
                AccountId = AccountId.GetValueOrDefault(),
                ActionBy = UserId.GetValueOrDefault()

            };

            var result = await Mediator.Send(request);
            return Respond(result);
        }

        [HttpDelete("{tournamentId}/team-group")]
        [ValidateModel]
        [Produces("application/json")]
        public async Task<IActionResult> DeleteTournamentTeamGroups(Guid tournamentId)
        {
            var request = new DeleteTournamentTeamGroups.Request
            {
                TournamentId = tournamentId,
                AccountId = AccountId.GetValueOrDefault(),
                ActionBy = UserId.GetValueOrDefault()
            };

            var result = await Mediator.Send(request);
            return Respond(result);
        }

        #endregion

        #region tournament round

        [HttpGet("{tournamentId}/round")]
        [ValidateModel]
        [Produces("application/json")]
        public async Task<IActionResult> GetTournamentRounds(Guid tournamentId)
        {
            var query = new GetTournamentRounds.Query
            {
                AccountId = AccountId.GetValueOrDefault(),
                TournamentId = tournamentId
            };

            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("{tournamentId}/round")]
        [ValidateModel]
        [Produces("application/json")]
        public async Task<IActionResult> AddTournamentRounds([FromBody] CreateTournamentRound.Request request, Guid tournamentId)
        {
            request.TournamentId = tournamentId;
            request.AccountId = AccountId.GetValueOrDefault();
            request.ActionBy = UserId.GetValueOrDefault();

            var result = await Mediator.Send(request);
            return Respond(result);
        }

        [HttpPut("{tournamentId}/round/{roundId}")]
        [ValidateModel]
        [Produces("application/json")]
        public async Task<IActionResult> UpdateTournamentRound([FromBody] UpdateTournamentRound.Request request, Guid tournamentId, Guid roundId)
        {
            request.Id = roundId;
            request.TournamentId = tournamentId;
            request.AccountId = AccountId.GetValueOrDefault();
            request.ActionBy = UserId.GetValueOrDefault();

            var result = await Mediator.Send(request);
            return Respond(result);
        }
        
        [HttpDelete("{tournamentId}/round")]
        [ValidateModel]
        [Produces("application/json")]
        public async Task<IActionResult> DeleteTournamentRounds(Guid tournamentId)
        {
            var request = new DeleteTournamentRound.Request
            {
                TournamentId = tournamentId,
                AccountId = AccountId.GetValueOrDefault(),
                ActionBy = UserId.GetValueOrDefault()
            };

            var result = await Mediator.Send(request);
            return Respond(result);
        }

        #endregion

    }
}
