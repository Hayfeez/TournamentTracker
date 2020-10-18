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
    public static class GetPlayersTeams
    {
        public class Query : IRequest<Result>
        {
            [JsonIgnore]
            public Guid UserId { get; set; }
            [JsonIgnore]
            public Guid AccountId { get; set; }

            public Guid PlayerId { get; set; }
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
            public Guid AccountId { get; set; }
            public Guid PlayerId { get; set; }
            public string TeamName { get; set; }
            public DateTime CreatedOn { get; set; }

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
                    .Where(x => x.AccountId == request.AccountId && x.PlayerId == request.PlayerId && !x.IsDeleted)
                    .Join(_readContext.Teams, teamPlayer => teamPlayer.AccountId, team => team.Id, (teamPlayer, team) => new
                    {
                        teamPlayer.PlayerId,
                        teamPlayer.Id,
                        teamPlayer.AccountId,
                        TeamName = team.Name,
                        teamPlayer.CreatedOn
                    })
                    .ProjectTo<Model>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken: cancellationToken);

                return new Result(items);
            }
        }
    }
}
