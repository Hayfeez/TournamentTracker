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

using Newtonsoft.Json;

using TournamentTracker.Common.Helpers;
using TournamentTracker.Data.Contexts;
using TournamentTracker.Infrastructure.BasicResults;

namespace TournamentTracker.Infrastructure.Commands.Tournaments
{
    public static class DeleteTournamentTeamGroups
    {
        public class Request : IRequest<Result>
        {
            public Guid ActionBy { get; set; }

            public Guid AccountId { get; set; }

            public Guid TournamentId { get; set; }

        }

        public class Result : BasicActionResult
        {
            public Guid Id { get; set; }

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
            private readonly TournamentTrackerWriteContext _readWriteContext;

            public Handler(TournamentTrackerWriteContext readContext)
            {
                _readWriteContext = readContext;
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var groups = _readWriteContext.TeamGroups.Where(x => x.AccountId == request.AccountId
                                                                     && x.TournamentId == request.TournamentId)
                    .ToList();

                if (groups.Count == 0)
                {
                    return new Result("This tournament has no team groups");
                }

                _readWriteContext.TeamGroups.RemoveRange(groups);

                return await _readWriteContext.SaveChangesAsync() > 0 ? new Result() : new Result(HttpStatusCode.BadRequest);
            }
        }
    }
}
