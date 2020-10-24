using System;
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

using Newtonsoft.Json;

using TournamentTracker.Common.Helpers;
using TournamentTracker.Data.Contexts;
using TournamentTracker.Infrastructure.BasicResults;

namespace TournamentTracker.Infrastructure.Commands.Tournaments
{
    public static class UpdateTournamentPrize
    {
        public class Request : IRequest<Result>
        {
            [JsonIgnore]
            public Guid ActionBy { get; set; }

            [JsonIgnore]
            public Guid AccountId { get; set; }

            [JsonIgnore]
            public Guid Id { get; set; }

            [JsonIgnore]
            public Guid TournamentId { get; set; }

            [Required]
            public string Name { get; set; }

            [Required]
            public decimal Amount { get; set; }
        }

        public class Result : BasicActionResult
        {
            public Guid Id { get; set; }

            public Result(string errorMessage) : base(errorMessage)
            {
            }

            public Result()
            {
                Status = HttpStatusCode.OK;
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
                var prizes = _readWriteContext.TournamentPrizes.Where(x => x.AccountId == request.AccountId
                                                                                    && x.TournamentId == request.TournamentId
                                                                                    && !x.IsDeleted).ToList();

                var prize = prizes.SingleOrDefault(x => x.Id == request.Id);

                if (prize == null)
                {
                    return new Result("Tournament prize cannot be found");
                }

                if (prize.IsPercentage)
                {
                    if ((prizes.Where(x => x.Id != request.Id).Sum(x => x.Amount) + request.Amount) > 80)
                    {
                        return new Result("Sum of all percentages cannot be more than 80%");
                    }
                }

                prize.Amount = request.Amount;
                prize.Name = request.Name;
              
                return await _readWriteContext.SaveChangesAsync() > 0 ? new Result() : new Result(HttpStatusCode.BadRequest);
            }
        }
    }
}
