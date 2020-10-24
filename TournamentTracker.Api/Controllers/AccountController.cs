using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using TournamentTracker.Api.ErrorLogger;
using TournamentTracker.Api.Filters;
using TournamentTracker.Infrastructure.Commands.Accounts;
using TournamentTracker.Infrastructure.Queries.Accounts;

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
        public async Task<IActionResult> GetAccounts()
        {
            var result = await Mediator.Send(new GetAccounts.Query());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ValidateModel]
        public async Task<IActionResult> GetAccount(Guid id)
        {
            var query = new GetAccountById.Query
            {
                Id = id
            };

            var result = await Mediator.Send(query);
            return Ok(result);
        }


        [HttpPost("")]
        [ValidateModel]
        public async Task<IActionResult> AddAccount([FromBody] CreateAccount.Request request)
        {
            request.ActionBy = UserId.GetValueOrDefault();

            var result = await Mediator.Send(request);
            return Respond(result);
        }

        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccount.Request request, Guid id)
        {
            request.Id = id;
            request.ActionBy = UserId.GetValueOrDefault();

            var result = await Mediator.Send(request);
            return Respond(result);
        }

        //[HttpDelete("{id}")]
        //[ValidateModel]
        //public async Task<IActionResult> DeleteAccount([FromBody] DeleteCommand.Request request, Guid id)
        //{
        //    request.Id = id;
        //   request.AccountId = AccountId.GetValueOrDefault();
        //  request.ActionBy = UserId.GetValueOrDefault();

        //    var result = await Mediator.Send(request);
        //    return Respond(result);
        //}
    }
}
