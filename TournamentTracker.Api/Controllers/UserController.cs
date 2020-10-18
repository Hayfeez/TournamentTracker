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
using TournamentTracker.Infrastructure.Commands.Players;
using TournamentTracker.Infrastructure.Commands.Users;
using TournamentTracker.Infrastructure.Queries.Players;
using TournamentTracker.Infrastructure.Queries.Users;

namespace TournamentTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UserController : CustomControllerBase
    {
        public UserController(IMediator mediator, ILoggerManager logger): base (mediator, logger)
        {
        }

        //[HttpGet("")]
        //[ValidateModel]
        //public async Task<IActionResult> GetUsers([FromQuery] GetUsers.Query query)
        //{
        //    query.AccountId = AccountId.GetValueOrDefault();
        //    var result = await Mediator.Send(query);
        //    return Ok(result);
        //}

        [HttpGet("account")]
        [ValidateModel]
        public async Task<IActionResult> GetUsersInAccount([FromQuery] GetUsersInAccount.Query query)
        {
            query.AccountId = AccountId.GetValueOrDefault();
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ValidateModel]
        public async Task<IActionResult> GetUser([FromQuery] GetUserById.Query query, Guid id)
        {
            query.Id = id;

            var result = await Mediator.Send(query);
            return Ok(result);
        }

        //[HttpGet("account/{id}")]
        //[ValidateModel]
        //public async Task<IActionResult> GetUser([FromQuery] GetUserAccountById.Query query, Guid id)
        //{
        //    query.AccountId = AccountId.GetValueOrDefault();
        //    query.Id = id;

        //    var result = await Mediator.Send(query);
        //    return Ok(result);
        //}

        [HttpGet("not-in-account")]
        [ValidateModel]
        public async Task<IActionResult> GetUsersNotInAccount([FromQuery] GetUsersNotInAccount.Query query)
        {
            query.AccountId = AccountId.GetValueOrDefault();

            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("account/{userId}")]
        [ValidateModel]
        public async Task<IActionResult> GetUsersAccounts([FromQuery] GetUsersAccounts.Query query, Guid userId)
        {
            query.UserId = userId;

            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("")]
        [ValidateModel]
        public async Task<IActionResult> AddUser([FromBody] CreateUser.Request request)
        {
            request.ActionBy = UserId.GetValueOrDefault();

            var result = await Mediator.Send(request);
            return Respond(result);
        }

        [HttpPost("account/assign")]
        [ValidateModel]
        public async Task<IActionResult> AssignUsersToAccount([FromBody] AssignUsersToAccount.Request request)
        {
            request.AccountId = AccountId.GetValueOrDefault();
            request.ActionBy = UserId.GetValueOrDefault();

            var result = await Mediator.Send(request);
            return Respond(result);
        }


        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUser.Request request, Guid id)
        {
            request.Id = id;
            request.ActionBy = UserId.GetValueOrDefault();
            request.AccountId = AccountId.GetValueOrDefault();

            var result = await Mediator.Send(request);
            return Respond(result);
        }

        //[HttpPut("account/{id}")]
        //[ValidateModel]
        //public async Task<IActionResult> UpdateUserAccount([FromBody] UpdateUserAccount.Request request, Guid id)
        //{
        //    request.Id = id;
        //    request.ActionBy = UserId.GetValueOrDefault();
        //    request.AccountId = AccountId.GetValueOrDefault();

        //    var result = await Mediator.Send(request);
        //    return Respond(result);
        //}

        [HttpDelete("")]
        [ValidateModel]
        public async Task<IActionResult> RemoveUserFromAccount([FromBody] RemoveUserFromAccount.Request request)
        {
            request.AccountId = AccountId.GetValueOrDefault();
            request.ActionBy = UserId.GetValueOrDefault();

            var result = await Mediator.Send(request);
            return Respond(result);
        }

    }
}
