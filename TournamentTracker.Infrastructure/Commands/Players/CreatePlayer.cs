using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using MediatR;

using Newtonsoft.Json;

using TournamentTracker.Common.Helpers;
using TournamentTracker.Data.Contexts;
using TournamentTracker.Data.Models;
using TournamentTracker.Infrastructure.BasicResults;

namespace TournamentTracker.Infrastructure.Commands.Players
{
    public static class CreatePlayer
    {
        public class Request : IRequest<Result>
        {
            [JsonIgnore]
            public Guid ActionBy { get; set; }

            [JsonIgnore]
            public Guid AccountId { get; set; }

            [Required]
            public string FirstName { get; set; }

            [Required]
            public string LastName { get; set; }

            [Required]
            public string PlayerNo { get; set; }

            public Guid? TeamId { get; set; }

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
                if (_readWriteContext.Players.Any(x => x.AccountId == request.AccountId 
                                                     && x.PlayerNo.ToLower() == request.PlayerNo.ToLower()))
                {
                    return new Result("Player with this number already exists");
                }

                var toAdd = _mapper.Map<Player>(request);
                toAdd.Id = SequentialGuid.Create();
                _readWriteContext.Players.Add(toAdd);

                if (request.TeamId.HasValue)
                {
                    var teamPlayer = _mapper.Map<TeamPlayer>(request);
                    teamPlayer.Id = SequentialGuid.Create();
                    teamPlayer.TeamId = request.TeamId.Value;
                    teamPlayer.PlayerId = toAdd.Id;

                    _readWriteContext.TeamPlayers.Add(teamPlayer);
                }

                return await _readWriteContext.SaveChangesAsync() > 0 ? new Result(toAdd.Id) : new Result(HttpStatusCode.BadRequest);
            }
        }
    }
}
