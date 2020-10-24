using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Newtonsoft.Json;

using TournamentTracker.Data.Contexts;
using TournamentTracker.Data.Models;

namespace TournamentTracker.Infrastructure.Commands.Players
{
    public static class DeletePlayer
    {
        public class Request : DeleteCommand.Request
        {

        }

        public class Handler : DeleteCommand.Handler<Player>, IRequestHandler<Request, DeleteCommand.Result>
        {
            public Handler(TournamentTrackerWriteContext readWriteContext) : base(readWriteContext)
            {
            }

            public async Task<DeleteCommand.Result> Handle(Request request, CancellationToken cancellationToken)
            {
                return await base.Handle(request, cancellationToken);
            }
        }
    }
}
