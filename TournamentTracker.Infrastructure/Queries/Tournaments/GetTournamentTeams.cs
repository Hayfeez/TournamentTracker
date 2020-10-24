using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

using TournamentTracker.Data.Contexts;

namespace TournamentTracker.Infrastructure.Queries.Tournaments
{
    public static class GetTournamentTeams
    {
        public class Query : IRequest<Result>
        {
            public Guid AccountId { get; set; }

            public Guid TournamentId { get; set; }

        }

        public class Result : List<Model>
        {
            public Result(List<Model> collection) : base(collection)
            {
            }
        }

        public class Model
        {
            public Guid Id { get; set; }
            public Guid TournamentId { get; set; }
            public string TeamName { get; set; }
            public string TeamCaptain { get; set; }

        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly IMapper _mapper;
            private readonly TournamentTrackerReadContext _readContext;

            public Handler(TournamentTrackerReadContext readContext, IMapper mapper)
            {
                _readContext = readContext;
                _mapper = mapper;
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                var items = await _readContext.TournamentTeams
                    .Where(x => x.AccountId == request.AccountId && x.TournamentId == request.TournamentId && !x.IsDeleted)
                    .Join(_readContext.Teams, tournamentTeam => tournamentTeam.TournamentId, team => team.Id, (tournamentTeam, team) => new
                    {
                        tournamentTeam.Id,
                        tournamentTeam.TournamentId,
                        team.Name,
                        team.AccountId,
                        team.TeamCaptain
                    })
                    .Join(_readContext.Players, team => team.TeamCaptain, player => player.Id, (team, player) => new
                    {
                        team.Id,
                        team.TournamentId,
                        team.Name,
                        player.AccountId,
                        player.FirstName,
                        player.LastName,
                        player.PlayerNo
                    })
                    .Select(x => new Model
                    {
                        TeamCaptain = x.FirstName + " " + x.LastName + " (" + x.PlayerNo + ")",
                        TeamName = x.Name,
                        Id = x.Id,
                        TournamentId = x.TournamentId
                    })
                   .ToListAsync(cancellationToken: cancellationToken);

                return new Result(items);
            }
        }
    }
}
