using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

using TournamentTracker.Data.Contexts;

namespace TournamentTracker.Infrastructure.Queries.Groups
{
    public static class GetGroupById
    {
        public class Query : IRequest<Result>
        {
            public Guid AccountId { get; set; }
            public Guid Id { get; set; }
        }


        public class Result
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
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
                var item = await _readContext.Groups
                    .SingleOrDefaultAsync(x => x.AccountId == request.AccountId && x.Id == request.Id && !x.IsDeleted);
                    

                return _mapper.Map<Result>(item);
            }
        }
    }
}
