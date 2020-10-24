using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

using TournamentTracker.Data.Contexts;

namespace TournamentTracker.Infrastructure.Queries.Players
{
    public static class GetPlayersInTeam
    {
        public class Query : IRequest<Result>
        {
            public Guid AccountId { get; set; }

            public Guid TeamId { get; set; }
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
            public Guid TeamId { get; set; }
            public Guid PlayerId { get; set; }
            public string TeamName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string PlayerNo { get; set; }

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
                var items = await _readContext.TeamPlayers
                    .Where(x => x.AccountId == request.AccountId && x.TeamId == request.TeamId && !x.IsDeleted)
                    .Join(_readContext.Players, teamPlayer => teamPlayer.PlayerId, player => player.Id, (teamPlayer, player) => new
                    {
                        teamPlayer.Id,
                        teamPlayer.PlayerId,
                        teamPlayer.TeamId,
                        player.FirstName,
                        player.LastName,
                        player.PlayerNo
                    })
                    .Join(_readContext.Teams, teamPlayer => teamPlayer.TeamId, team => team.Id, (teamPlayer, team) => new
                    {
                        TeamName = team.Name,
                        TeamId = team.Id,
                        TeamPlayerId = teamPlayer.Id,
                        teamPlayer.PlayerId,
                        teamPlayer.FirstName,
                        teamPlayer.LastName,
                        teamPlayer.PlayerNo
                    })
                    .Select(x => new Model
                    {
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        PlayerNo = x.PlayerNo,
                        Id = x.TeamPlayerId,
                        PlayerId = x.PlayerId,
                        TeamId = x.TeamId,
                        TeamName = x.TeamName
                    })
                   .ToListAsync(cancellationToken: cancellationToken);

                return new Result(items);
            }
        }
    }
}
