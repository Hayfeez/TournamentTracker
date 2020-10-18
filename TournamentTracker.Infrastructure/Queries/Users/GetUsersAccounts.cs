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

namespace TournamentTracker.Infrastructure.Queries.Users
{
    public static class GetUsersAccounts
    {
        public class Query : IRequest<Result>
        {
            [JsonIgnore]
            public Guid UserId { get; set; }
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
            public Guid UserId { get; set; }
            public string AccountName { get; set; }
            public string Domain { get; set; }
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
                var items = await _readContext.UserAccounts
                    .Where(x => x.UserId == request.UserId && !x.IsDeleted)
                    .Join(_readContext.Accounts, userAccount => userAccount.AccountId, account => account.Id, (userAccount, account) => new
                    {
                        userAccount.UserId,
                        userAccount.Id,
                        userAccount.AccountId,
                        AccountName = account.Name,
                        account.Domain,
                        userAccount.CreatedOn
                    })
                    .ProjectTo<Model>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken: cancellationToken);

                return new Result(items);
            }
        }
    }
}
