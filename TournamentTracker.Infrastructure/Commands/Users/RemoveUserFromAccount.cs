using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;

using TournamentTracker.Data.Models;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

using TournamentTracker.Common.Helpers;
using TournamentTracker.Data.Contexts;
using TournamentTracker.Infrastructure.BasicResults;

namespace TournamentTracker.Infrastructure.Commands.Users
{
    public static class RemoveUserFromAccount
    {
        public class Request : IRequest<Result>
        {
            public Guid ActionBy { get; set; }

            public Guid AccountId { get; set; }

            public Guid UserId { get; set; }

        }

        public class Result : BasicActionResult
        {
            public Result(string errorMessage) : base(errorMessage)
            {
            }

            public Result()
            {
                Status = HttpStatusCode.OK;
            }

            public Result(HttpStatusCode status) : base(status)
            {
            }
        }

        public class Handler : IRequestHandler<Request, Result>
        {
            private readonly IMapper _mapper;
            private readonly TournamentTrackerWriteContext _readWriteContext;

            public Handler(TournamentTrackerWriteContext readContext, IMapper mapper)
            {
                _readWriteContext = readContext;
                _mapper = mapper;
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var item = _readWriteContext.UserAccounts.SingleOrDefault(x => x.AccountId == request.AccountId
                                                                              && x.UserId == request.UserId);

                if (item == null)
                {
                    return new Result(HttpStatusCode.NotFound);
                }

                item.IsDeleted = true;
                item.DeletedOn = DateTime.Now;

                return await _readWriteContext.SaveChangesAsync() > 0 ? new Result() : new Result(HttpStatusCode.BadRequest);
            }
        }
    }
}
