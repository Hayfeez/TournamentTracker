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
using AutoMapper.QueryableExtensions;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

using TournamentTracker.Common.Helpers;
using TournamentTracker.Data.Contexts;
using TournamentTracker.Infrastructure.BasicResults;

namespace TournamentTracker.Infrastructure.Commands.Teams
{
    public static class CreateTeam
    {
        public class Request : IRequest<Result>
        {
            [JsonIgnore]
            public Guid ActionBy { get; set; }

            [JsonIgnore]
            public Guid AccountId { get; set; }

            [Required]
            public string Name { get; set; }


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
                if (_readWriteContext.Teams.Any(x => x.AccountId == request.AccountId 
                                                     && x.Name.ToLower() == request.Name.ToLower()))
                {
                    return new Result("Team name already exists");
                }

                var toAdd = _mapper.Map<Team>(request);
                toAdd.Id = SequentialGuid.Create();
                _readWriteContext.Teams.Add(toAdd);

                return await _readWriteContext.SaveChangesAsync() > 0 ? new Result(toAdd.Id) : new Result(HttpStatusCode.BadRequest);
            }
        }
    }
}
