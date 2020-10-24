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

namespace TournamentTracker.Infrastructure.Queries.Users
{
    public static class GetUsersInAccount
    {
        public class Query : IRequest<Result>
        {
            public Guid AccountId { get; set; }

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
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }

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
                    .Where(x => x.AccountId == request.AccountId && !x.IsDeleted)
                    .Join(_readContext.Users, userAccount => userAccount.UserId, user => user.Id, (userAccount, user) => new
                    {
                        UserAccountId = userAccount.Id,
                        userAccount.UserId,
                        userAccount.AccountId,
                        user.FirstName,
                        user.LastName,
                        user.Email
                    })
                    .Select(x => new Model
                    {
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Email = x.Email,
                        Id = x.UserAccountId,
                        AccountId = x.AccountId,
                        UserId = x.UserId
                    })
                   .ToListAsync(cancellationToken: cancellationToken);

                return new Result(items);
            }
        }
    }
}
