﻿using System;
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

namespace TournamentTracker.Infrastructure.Queries.Teams
{
    public static class GetTeams
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
            public string Name { get; set; }
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
                var items = await _readContext.Teams
                    .Where(x => x.AccountId == request.AccountId && !x.IsDeleted)
                    .Select(x => new Model
                    {
                        CreatedOn = x.CreatedOn.Value,
                        Name = x.Name,
                        Id = x.Id
                    })
                    .OrderBy(x=>x.Name)
                   .ToListAsync(cancellationToken: cancellationToken);

                return new Result(items);
            }
        }
    }
}
