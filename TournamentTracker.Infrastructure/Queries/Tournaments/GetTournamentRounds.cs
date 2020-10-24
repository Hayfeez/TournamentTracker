using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TournamentTracker.Data.Models;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

using TournamentTracker.Data.Contexts;

namespace TournamentTracker.Infrastructure.Queries.Tournaments
{
    public static class GetTournamentRounds
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

            public string Round { get; set; }

            public int RoundRank { get; set; }

            public int TeamsInRound { get; set; }

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
                var list = await _readContext.TournamentRounds
                    .Where(x => x.AccountId == request.AccountId && x.TournamentId == request.TournamentId)
                    .ProjectTo<Model>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken: cancellationToken);

                return new Result(list);
            }
        }
    }
}
