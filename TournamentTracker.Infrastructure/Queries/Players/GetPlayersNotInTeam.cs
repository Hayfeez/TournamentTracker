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
    public static class GetPlayersNotInTeam
    {
        public class Query : IRequest<Result>
        {
            [JsonIgnore]
            public Guid AccountId { get; set; }

            [JsonIgnore]
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
                var items = await (from player in _readContext.Players
                                   where player.AccountId == request.AccountId
                                         && !player.IsDeleted
                                         && !(from teamPlayer in _readContext.TeamPlayers
                                              where teamPlayer.AccountId == request.AccountId 
                                                    && teamPlayer.TeamId == request.TeamId
                                              select teamPlayer.PlayerId).Contains(player.Id)
                                   select new Model
                                   {
                                       FirstName = player.FirstName,
                                       LastName = player.LastName,
                                       Id = player.Id,
                                       PlayerNo = player.PlayerNo
                                   })
                    .ToListAsync(cancellationToken: cancellationToken);

                return new Result(items);
            }
        }
    }
}
