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

namespace TournamentTracker.Infrastructure.Queries.Tournaments
{
    public static class GetTournamentGroupById
    {
        public class Query : IRequest<Result>
        {
            public Guid AccountId { get; set; }

            public Guid Id { get; set; }

            public Guid TournamentId { get; set; }

        }

        public class Result : Model
        {
        }

        public class Model
        {
            public Guid Id { get; set; }
            public Guid AccountId { get; set; }
            public Guid TournamentId { get; set; }
            public string GroupName { get; set; }

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
                var item = await _readContext.TournamentGroups
                    .Where(x => x.AccountId == request.AccountId && x.Id == request.Id)
                    .Join(_readContext.Groups, tournamentGroup => tournamentGroup.TournamentId, group => group.Id, (tournamentGroup, group) => new
                    {
                        tournamentGroup.Id,
                        tournamentGroup.TournamentId,
                        group.Name,
                        group.AccountId
                    })
                    .Select(x => new Model
                    {
                        GroupName = x.Name,
                        Id = x.Id,
                        AccountId = x.AccountId,
                        TournamentId = x.TournamentId
                    })
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);

                return _mapper.Map<Result>(item);
            }
        }
    }
}
