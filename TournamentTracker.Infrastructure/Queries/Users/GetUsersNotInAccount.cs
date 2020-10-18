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
    public static class GetUsersNotInAccount
    {
        public class Query : IRequest<Result>
        {
            [JsonIgnore]
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
                var items = await (from user in _readContext.Users
                                   where !(from userAccount in _readContext.UserAccounts
                                              where userAccount.AccountId == request.AccountId
                                              select userAccount.UserId).Contains(user.Id)
                                   select new Model
                                   {
                                       FirstName = user.FirstName,
                                       LastName = user.LastName,
                                       Id = user.Id,
                                       Email = user.Email
                                   })
                    .ToListAsync(cancellationToken: cancellationToken);

                return new Result(items);
            }
        }
    }
}
