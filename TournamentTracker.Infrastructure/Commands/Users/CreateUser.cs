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

namespace TournamentTracker.Infrastructure.Commands.Users
{
    public static class CreateUser
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
            public string Email { get; set; }

        }

        public class Result : BasicActionResult
        {
            public Guid Id { get; set; }

            public Result()
            {
                    
            }
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
                var user = _readWriteContext.Users.SingleOrDefault(x => string.Equals(x.Email, request.Email, StringComparison.CurrentCultureIgnoreCase));
                if (user == null) // user doesn't exist
                {
                    var toAdd = _mapper.Map<User>(request);
                    toAdd.Id = SequentialGuid.Create();
                    _readWriteContext.Users.Add(toAdd);

                    AddUserAccount(request);
                }
                else
                {
                    if (!(string.Equals(user.FirstName, request.FirstName, StringComparison.CurrentCultureIgnoreCase)
                          && string.Equals(user.LastName, request.LastName, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        return new Result("Email address is in use");
                    }

                    var userAccount = _readWriteContext.UserAccounts.SingleOrDefault(x => x.AccountId == request.AccountId && x.UserId == user.Id);
                   
                    if(userAccount == null)
                    {
                        AddUserAccount(request);
                    }
                    else
                    {
                        return new Result("User already exists for this account");
                    }
                }

                return await _readWriteContext.SaveChangesAsync() > 0 ? new Result() : new Result(HttpStatusCode.BadRequest);
            }

            private void AddUserAccount(Request request)
            {
                var toAdd = _mapper.Map<UserAccount>(request);
                toAdd.Id = SequentialGuid.Create();
                _readWriteContext.UserAccounts.Add(toAdd);
            }
        }
    }
}
