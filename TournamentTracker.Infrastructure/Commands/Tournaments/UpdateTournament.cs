﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;

using TournamentTracker.Data.Models;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

using TournamentTracker.Common.Helpers;
using TournamentTracker.Data.Attributes;
using TournamentTracker.Data.Contexts;
using TournamentTracker.Infrastructure.BasicResults;

namespace TournamentTracker.Infrastructure.Commands.Tournaments
{
    public static class UpdateTournament
    {
        public class Request : IRequest<Result>
        {
            [JsonIgnore]
            public Guid ActionBy { get; set; }

            [JsonIgnore]
            public Guid AccountId { get; set; }

            [JsonIgnore]
            public Guid Id { get; set; }

            [Required]
            public string Name { get; set; }

            public bool LowestScoreWins { get; set; }

            public decimal EntryFee { get; set; }

            public bool HasGroupStage { get; set; }

            [RequiredIf(nameof(HasGroupStage), true)]
            public int TeamPerGroup { get; set; }
        }

        public class Result : BasicActionResult
        {
            public Result(string errorMessage) : base(errorMessage)
            {
            }

            public Result(HttpStatusCode status) : base(status)
            {
            }
        }

        public class Handler : IRequestHandler<Request, Result>
        {
            private readonly IMapper _mapper;
            private readonly TournamentTrackerWriteContext _readWriteContext;

            public Handler(TournamentTrackerWriteContext readContext, IMapper mapper)
            {
                _readWriteContext = readContext;
                _mapper = mapper;
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var item = _readWriteContext.Tournaments.SingleOrDefault(x => x.Id == request.Id && x.AccountId == request.AccountId && !x.IsDeleted);
                if (item == null)
                {
                    return new Result(HttpStatusCode.NotFound);
                }


                if (_readWriteContext.Tournaments.Any(x => x.Id != request.Id 
                                                           && x.AccountId == request.AccountId
                                                           && x.Name.ToLower() == request.Name.ToLower()))
                {
                    return new Result("Tournament name already exists");
                }

                _mapper.Map(request, item);

                return await _readWriteContext.SaveChangesAsync() > 0 ? new Result(HttpStatusCode.NoContent) : new Result(HttpStatusCode.BadRequest);
            }
        }
    }
}
