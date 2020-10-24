using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

using TournamentTracker.Data.Contexts;

namespace TournamentTracker.Infrastructure.Queries.Users
{
    public static class GetUserAccount
    {
        public class Query : IRequest<Result>
        {
            public Guid AccountId { get; set; }
            public Guid UserId { get; set; }
        }

        public class Result : Model
        {
            public Result(Model model)
            {
            }
        }

        public class Model
        {
            public Guid Id { get; set; }

            public Guid AccountId { get; set; }

            public Guid UserId { get; set; }

            public bool IsPending { get; set; }
            public bool IsDeleted { get; set; }

        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly TournamentTrackerReadContext _readContext;
            private readonly IMapper _mapper;

            public Handler(TournamentTrackerReadContext readContext, IMapper mapper)
            {
                _readContext = readContext;
                _mapper = mapper;
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                var item = await _readContext.UserAccounts
                    .Where(x => x.AccountId == request.AccountId && x.UserId == request.UserId && !x.IsDeleted)
                    .ProjectTo<Model>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken);

                return new Result(item);
            }
        }
    }
}
