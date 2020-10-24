using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using MediatR;

using Newtonsoft.Json;

using TournamentTracker.Data.Contexts;
using TournamentTracker.Data.Models;
using TournamentTracker.Infrastructure.BasicResults;
using TournamentTracker.Infrastructure.Helpers;

namespace TournamentTracker.Infrastructure.Commands.Tournaments
{
    public static class AddTournamentTeamsToGroups
    {
        public class Request : IRequest<Result>
        {
            public Guid ActionBy { get; set; }

            public Guid AccountId { get; set; }

            public Guid TournamentId { get; set; }


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
            private readonly IRandomizeHelper _randomGen;

            public Handler(TournamentTrackerWriteContext readContext, IMapper mapper, IRandomizeHelper randomGen)
            {
                _readWriteContext = readContext;
                _mapper = mapper;
                _randomGen = randomGen;
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var tournament = _readWriteContext.Tournaments.SingleOrDefault(x => x.Id == request.TournamentId && x.AccountId == request.AccountId && !x.IsDeleted);

                if (tournament == null)
                {
                    return new Result("Tournament cannot be found");
                }

                if (!tournament.HasGroupStage)
                {
                    return new Result("Tournament does not include a group stage");
                }

                if (_readWriteContext.TeamGroups.Any(x => x.AccountId == request.AccountId && x.TournamentId == request.TournamentId))
                {
                    return new Result("Delete all existing team groups");
                }

                var teams = _readWriteContext.TournamentTeams
                    .Where(x => x.AccountId == request.AccountId 
                                && x.TournamentId == request.TournamentId 
                                && !x.IsDeleted)
                    .ToList();

                var groups = _readWriteContext.TournamentGroups
                    .Where(x => x.AccountId == request.AccountId
                                && x.TournamentId == request.TournamentId)
                    .ToList();

                var teamPerGroup = tournament.TeamPerGroup;
                int noOfGroupsNeeded = teams.Count / teamPerGroup;

                if (teams.Count % teamPerGroup != 0)
                {
                    return new Result("There are no enough teams to match the required no of teams per group");
                }
                
                if (noOfGroupsNeeded > groups.Count)  
                {
                    return new Result("There are no enough group to place every team");
                }

                if (noOfGroupsNeeded < groups.Count)
                {
                    return new Result("There are too many groups, delete unneeded groups and try again");
                }

                var teamIds = teams.Select(x => x.Id).ToList();
                var groupIds = groups.Select(x => x.Id).ToList();

                var teamGroups = _randomGen.AddTeamsToGroups(request.AccountId, request.TournamentId, teamIds, groupIds, teamPerGroup, noOfGroupsNeeded);

                if (teamGroups.Count != teamPerGroup * noOfGroupsNeeded)
                {
                    return new Result("An error occured. Please try again");
                }

                _readWriteContext.TeamGroups.AddRange(teamGroups);
                return await _readWriteContext.SaveChangesAsync() > 0 ? new Result() : new Result(HttpStatusCode.BadRequest);
            }

        }
    }
}
