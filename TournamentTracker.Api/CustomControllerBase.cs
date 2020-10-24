using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using TournamentTracker.Api.ErrorLogger;
using TournamentTracker.Api.Filters;
using TournamentTracker.Infrastructure.BasicResults;
using TournamentTracker.Infrastructure.Queries.Users;

namespace TournamentTracker.Api
{
    public interface ICustomControllerBase
    {
        Guid? AccountId { get; set; }

        Guid? UserId { get; set; }

        IMediator Mediator { get; }

        Task<Guid> GetUserAccountId();

        Task<Guid> GetAccountId();
    }

    [AccountScopeFilter]
    [UserAuthorizationFilter]
    public class CustomControllerBase : ControllerBase, ICustomControllerBase
    {
        private Guid _userAccountId;

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

        [Microsoft.AspNetCore.Mvc.NonAction]
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

        [Microsoft.AspNetCore.Mvc.NonAction]
        public async Task<Guid> GetAccountId()
        {
            await GetUserAccountId();
           
            return AccountId.Value;
        }

        [Microsoft.AspNetCore.Mvc.NonAction]
        public async Task<Guid> GetUserAccountId()
        {
            if (_userAccountId != default(Guid))
            {
                return _userAccountId;
            }

            if (UserId == null)
            {
                throw new Exception("No user available on this request, ensure that the user is authorized");
            }

            var userAccount = await Mediator.Send(new GetUserAccount.Query
            {
                AccountId = AccountId.GetValueOrDefault(),
                UserId = UserId.GetValueOrDefault()
            });

           
            if (userAccount == null)
            {
                throw new Exception("No user account available on this request, ensure that the user is authorized and that the account scope is correct via the AccountId header.");
            }

            if (userAccount.IsDeleted)
            {
                throw new Exception("User has been marked inactive with this account. Re-activate user in order to access this account.");
            }

            AccountId = userAccount.AccountId;

            //if (userAccount.IsPending)
            //{
            //    await Mediator.Send(new ConfirmPendingUser.Command
            //    {
            //        AccountId = userAccount.AccountId,
            //        UserId = userAccount.UserId,
            //    });
            //}

            return _userAccountId = userAccount.Id;
        }

    }
}
