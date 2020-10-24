using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using MediatR;

using Newtonsoft.Json;

using TournamentTracker.Data.Contexts;
using TournamentTracker.Infrastructure.BasicResults;

namespace TournamentTracker.Infrastructure.Commands.Players
{
    public static class UpdatePlayer
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
            public string FirstName { get; set; }

            [Required]
            public string LastName { get; set; }

            [Required]
            public string PlayerNo { get; set; }

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
                var item = _readWriteContext.Players.SingleOrDefault(x => x.Id == request.Id && x.AccountId == request.AccountId && !x.IsDeleted);
                if (item == null)
                {
                    return new Result(HttpStatusCode.NotFound);
                }

                if (_readWriteContext.Players.Any(x => x.Id != request.Id
                                                     && x.AccountId == request.AccountId
                                                     && x.PlayerNo.ToLower() == request.PlayerNo.ToLower()))
                {
                    return new Result("Player with this number already exists");
                }

                _mapper.Map(request, item);

                return await _readWriteContext.SaveChangesAsync() > 0 ? new Result(HttpStatusCode.NoContent) : new Result(HttpStatusCode.BadRequest);
            }
        }
    }
}
