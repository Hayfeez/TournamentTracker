﻿using System;
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
    public static class GetUserAccountById
    {
        public class Query : IRequest<Result>
        {
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
                var item = await _readContext.Accounts
                    .Where(x => x.Id == request.Id && !x.IsDeleted)
                    .ProjectTo<Model>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);

                return new Result(item); //_mapper.Map<Result>(item);
            }
        }
    }
}
