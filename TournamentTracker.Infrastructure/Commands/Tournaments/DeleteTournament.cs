using System.Threading;
using System.Threading.Tasks;

using MediatR;

using TournamentTracker.Data.Contexts;
using TournamentTracker.Data.Models;

namespace TournamentTracker.Infrastructure.Commands.Tournaments
{
    public static class DeleteTournament
    {
        public class Request : DeleteCommand.Request
        {

        }

        public class Handler : DeleteCommand.Handler<Tournament>, IRequestHandler<Request, DeleteCommand.Result>
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
