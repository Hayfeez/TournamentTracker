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
    public static class GetUserById
    {
        public class Query : IRequest<Result>
        {
            [JsonIgnore]
            public Guid Id { get; set; }
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
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
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
                var item = await _readContext.Users
                    .Where(x => x.Id == request.Id)
                    .ProjectTo<Model>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);

                return new Result(item); //_mapper.Map<Result>(item);
            }
        }
    }
}
