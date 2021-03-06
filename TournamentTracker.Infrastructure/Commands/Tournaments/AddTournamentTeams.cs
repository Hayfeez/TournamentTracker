﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using MediatR;

using Newtonsoft.Json;

using TournamentTracker.Common.Helpers;
using TournamentTracker.Data.Contexts;
using TournamentTracker.Data.Models;
using TournamentTracker.Infrastructure.BasicResults;

namespace TournamentTracker.Infrastructure.Commands.Tournaments
{
    public static class AddTournamentTeams
    {
        public class Request : IRequest<Result>
        {
            [JsonIgnore]
            public Guid ActionBy { get; set; }

            [JsonIgnore]
            public Guid AccountId { get; set; }

            [JsonIgnore]
            public Guid TournamentId { get; set; }

            [Required]
            public List<Guid> TeamIds { get; set; }

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
                foreach (var teamId in request.TeamIds)
                {
                    if (_readWriteContext.TournamentTeams.Any(x => x.AccountId == request.AccountId
                                                               && x.TournamentId == request.TournamentId 
                                                               && x.TeamId == teamId
                                                               && !x.IsDeleted))
                    {
                        continue;
                    }

                    _readWriteContext.TournamentTeams.Add(new TournamentTeam
                    {
                        AccountId = request.AccountId,
                        TeamId = teamId,
                        Id = SequentialGuid.Create(),
                        TournamentId = request.TournamentId
                    });
                }

                return await _readWriteContext.SaveChangesAsync() > 0 ? new Result() : new Result(HttpStatusCode.BadRequest);
            }
        }
    }
}
