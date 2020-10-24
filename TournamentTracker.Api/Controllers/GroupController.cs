using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using TournamentTracker.Api.ErrorLogger;
using TournamentTracker.Api.Filters;
using TournamentTracker.Infrastructure.Commands.Groups;
using TournamentTracker.Infrastructure.Queries.Groups;


namespace TournamentTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class GroupController : CustomControllerBase
    {
        public GroupController(IMediator mediator, ILoggerManager logger): base (mediator, logger)
        {
        }

        [HttpGet("")]
        [ValidateModel]
        public async Task<IActionResult> GetGroups()
        {
            var query = new GetGroups.Query
            {
                AccountId = AccountId.GetValueOrDefault()
            };

            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ValidateModel]
        public async Task<IActionResult> GetGroup(Guid id)
        {
            var query = new GetGroupById.Query
            {
                Id = id,
                AccountId = AccountId.GetValueOrDefault()
            };

            var result = await Mediator.Send(query);
            return Ok(result);
        }


        [HttpPost("")]
        [ValidateModel]
        public async Task<IActionResult> AddGroup([FromBody] CreateGroup.Request request)
        {
            request.AccountId = AccountId.GetValueOrDefault();
            request.ActionBy = UserId.GetValueOrDefault();

            var result = await Mediator.Send(request);
            return Respond(result);
        }

        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateGroup([FromBody] UpdateGroup.Request request, Guid id)
        {
            request.Id = id;
            request.AccountId = AccountId.GetValueOrDefault();
            request.ActionBy = UserId.GetValueOrDefault();

            var result = await Mediator.Send(request);
            return Respond(result);
        }

        //[HttpDelete("{id}")]
        //[ValidateModel]
        //public async Task<IActionResult> DeleteGroup([FromBody] DeleteCommand.Request request, Guid id)
        //{
        //    request.Id = id;
        //    request.AccountId = AccountId.GetValueOrDefault();
        //    request.ActionBy = UserId.GetValueOrDefault();

        //    var result = await Mediator.Send(request);
        //    return Respond(result);
        //}
    }
}
