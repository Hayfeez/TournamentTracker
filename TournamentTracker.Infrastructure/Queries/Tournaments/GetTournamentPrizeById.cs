﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TournamentTracker.Data.Models;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

using TournamentTracker.Data.Contexts;

namespace TournamentTracker.Infrastructure.Queries.Tournaments
{
    public static class GetTournamentPrizeById
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
            public Guid TournamentId { get; set; }
            public bool IsPercentage { get; set; }

            public string Name { get; set; }

            public int Rank { get; set; }

            public decimal Amount { get; set; }
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
                var tournaments = await _readContext.TournamentPrizes
                    .Where(x => x.AccountId == request.AccountId && x.Id == request.Id && !x.IsDeleted)
                    .ProjectTo<Model>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);

                return _mapper.Map<Result>(tournaments);
            }
        }
    }
}
