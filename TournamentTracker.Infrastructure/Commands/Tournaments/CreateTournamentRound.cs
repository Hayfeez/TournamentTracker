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
    public static class CreateTournamentRound
    {
        public class Request : IRequest<Result>
        {
            [JsonIgnore]
            public Guid ActionBy { get; set; }

            [JsonIgnore]
            public Guid AccountId { get; set; }

            [JsonIgnore]
            public Guid TournamentId { get; set; }


            [Required]
            public List<RoundModel> Rounds { get; set; }
        }

        public class RoundModel
        {
            [Required]
            public string Round { get; set; }

            [Required]
            public int RoundRank { get; set; }

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

            bool IsPositivePowerOfTwo(int x)
            {
                return (x > 0) && ((x & (x - 1)) == 0);
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
               var tournament = _readWriteContext.Tournaments.SingleOrDefault(x => x.AccountId == request.AccountId
                                                                        && x.Id == request.TournamentId
                                                                        && !x.IsDeleted);
                if (tournament == null)
                {
                    return new Result("Tournament cannot be found");
                }

                if (_readWriteContext.TournamentGroups.Any(x => x.AccountId == request.AccountId
                                                                && x.TournamentId == request.TournamentId))
                {
                    return new Result("Tournament rounds already saved. Update or remove individual rounds instead");
                }

                var maxRank = request.Rounds.Max(x => x.RoundRank);
                //  var minRank = request.Rounds.Min(x => x.RoundRank);

                if (request.Rounds.Select(x=>x.RoundRank).Distinct().Count() != maxRank)
                {
                    return new Result("Round rank cannot contain duplicates");
                }

                if (request.Rounds.Count != maxRank) //since ranks start from 1, count should be equal to maxRank
                {
                    return new Result("Round rank must be consecutive positive numbers starting from 1");
                }

                var teamsInRound1 = Math.Pow(2, request.Rounds.Count);

                var teamCount = _readWriteContext.TournamentTeams.Count(x => x.AccountId == request.AccountId && x.TournamentId == request.TournamentId && !x.IsDeleted);

                if (teamCount > teamsInRound1 && !tournament.HasGroupStage)
                {
                    return new Result("Teams are greater than the number required in first round");
                }

                if (teamCount < teamsInRound1 && tournament.HasGroupStage)
                {
                    return new Result("Too few teams. Add more before you can continue");
                }

                var maxComputerTeam = teamsInRound1 > 8 ? 3 : 0; // for round 1 greater than 8 teams, there can be a maximum of 3 computer teams
                if (!tournament.HasGroupStage && teamCount < teamsInRound1 - maxComputerTeam)
                {
                    return new Result("Too few teams. Add more before you can continue");
                }

                request.Rounds = request.Rounds.OrderBy(x => x.RoundRank).ToList();
                int teamsInRound = (int)teamsInRound1;
                foreach (var round in request.Rounds)
                {
                    _readWriteContext.TournamentRounds.Add(new TournamentRound
                    {
                        Id = SequentialGuid.Create(),
                        AccountId = request.AccountId,
                        TournamentId = request.TournamentId,
                        RoundRank = round.RoundRank,
                        Round = round.Round,
                        TeamsInRound = teamsInRound
                    });

                    teamsInRound /= 2;
                }
                
                return await _readWriteContext.SaveChangesAsync() > 0 ? new Result() : new Result(HttpStatusCode.BadRequest);
            }
        }
    }
}
