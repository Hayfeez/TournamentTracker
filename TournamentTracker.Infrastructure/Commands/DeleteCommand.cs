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
using TournamentTracker.Data.Interfaces;
using TournamentTracker.Infrastructure.BasicResults;

namespace TournamentTracker.Infrastructure.Commands
{
    public static class DeleteCommand
    {
        public class Request : IRequest<Result>
        {
            [JsonIgnore]
            public Guid ActionBy { get; set; }

            [JsonIgnore]
            public Guid AccountId { get; set; }

            [JsonIgnore]
            public Guid Id { get; set; }

        }

        public class Result : BasicActionResult
        {
            public Result(HttpStatusCode status) : base(status)
            {
            }
        }

        public abstract class Handler<T> where T : class, IEntity, IAccount, IDeletable
        {
            private readonly TournamentTrackerWriteContext _readWriteContext;

            public Handler(TournamentTrackerWriteContext readContext)
            {
                _readWriteContext = readContext;
            }

            protected virtual T GetEntity<TRequest>(TRequest request) where TRequest : Request
            {
                return _readWriteContext.Set<T>().FirstOrDefault(w => w.AccountId == request.AccountId && w.Id == request.Id);
            }

            public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
            {
                var item = GetEntity(request);
                if (item == null)
                {
                    return new Result(HttpStatusCode.NotFound);
                }

                if (item.IsDeleted)
                {
                    return new Result(HttpStatusCode.NotFound);
                }

                item.IsDeleted = true;
                item.DeletedOn = DateTime.Now;

                return await _readWriteContext.SaveChangesAsync() > 0 ? new Result(HttpStatusCode.NoContent) : new Result(HttpStatusCode.BadRequest);
            }
        }
    }
}
