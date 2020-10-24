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
    public static class CreateTournamentPrize
    {
        public class Request : IRequest<Result>
        {
            [JsonIgnore]
            public Guid ActionBy { get; set; }

            [JsonIgnore]
            public Guid AccountId { get; set; }

            [JsonIgnore]
            public Guid TournamentId { get; set; }

            public bool IsPercentage { get; set; }

            [Required]
            public List<PrizeModel> Prizes { get; set; }
        }

        public class PrizeModel
        {
            [Required]
            public string Name { get; set; }

            [Required]
            public int Rank { get; set; }

            [Required]
            public decimal Amount { get; set; }

        }

        public class Result : BasicActionResult
        {
            public Guid Id { get; set; }

            public Result(string errorMessage) : base(errorMessage)
            {
            }

            public Result(Guid id)
            {
                Id = id;
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

                if (_readWriteContext.Tournaments.Any(x => x.AccountId == request.AccountId
                                                           && x.Id == request.TournamentId
                                                           && !x.IsDeleted))
                {
                    return new Result("Tournament cannot be found");
                }

                if (_readWriteContext.TournamentPrizes.Any(x => x.AccountId == request.AccountId
                                                                && x.TournamentId == request.TournamentId
                                                                && !x.IsDeleted))
                {
                    return new Result("Tournament prize already saved. Update or remove individual prizes instead");
                }

                var maxRank = request.Prizes.Max(x => x.Rank);
                //  var minRank = request.Prizes.Min(x => x.Rank);

                if (request.Prizes.Select(x=>x.Rank).Distinct().Count() != maxRank)
                {
                    return new Result("Prize rank cannot contain duplicates");
                }

                if (request.Prizes.Count != maxRank) //since ranks start from 1, count should be equal to maxRank
                {
                    return new Result("Prize rank must be consecutive positive numbers");
                }

                if (request.IsPercentage && (request.Prizes.Sum(x => x.Amount) > 80))
                {
                    return new Result("Sum of all percentages cannot be more than 80%");
                }

                request.Prizes = request.Prizes.OrderBy(x => x.Rank).ToList();
                foreach (var prize in request.Prizes)
                {
                    _readWriteContext.TournamentPrizes.Add(new TournamentPrize
                    {
                        Id = SequentialGuid.Create(),
                        AccountId = request.AccountId,
                        TournamentId = request.TournamentId,
                        IsPercentage = request.IsPercentage,
                        Rank = prize.Rank,
                        Name = prize.Name,
                        Amount = prize.Amount
                    });
                }
                
                return await _readWriteContext.SaveChangesAsync() > 0 ? new Result(request.TournamentId) : new Result(HttpStatusCode.BadRequest);
            }
        }
    }
}
