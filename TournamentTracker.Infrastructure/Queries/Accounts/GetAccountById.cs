﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

using TournamentTracker.Data.Contexts;

namespace TournamentTracker.Infrastructure.Queries.Accounts
{
    public static class GetAccountById
    {
        public class Query : IRequest<Result>
        {
            public Guid Id { get; set; }
        }


        public class Result
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
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
                    .Where(x =>  x.Id == request.Id && !x.IsDeleted)
                    .Select(x => new Result
                    {
                        Domain = x.Domain,
                        CreatedOn = x.CreatedOn.Value,
                        Name = x.Name,
                        Id = x.Id
                    })
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);

                return item;
            }
        }
    }
}
