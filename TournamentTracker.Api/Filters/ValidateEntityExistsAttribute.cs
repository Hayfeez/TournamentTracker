using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using Newtonsoft.Json;

using TournamentTracker.Data.Contexts;
using TournamentTracker.Data.Interfaces;

namespace TournamentTracker.Api.Filters
{
    public class ValidateEntityExistsAttribute<T> : IActionFilter where T : class, IEntity
    {
        private readonly TournamentTrackerReadContext _readContext;

        public ValidateEntityExistsAttribute(TournamentTrackerReadContext readContext)
        {
            _readContext = readContext;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
           
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var id = Guid.Empty;

            if (context.ActionArguments.ContainsKey("id"))
            {
                id = (Guid)context.ActionArguments["id"];
            }
            else
            {
                context.Result = new BadRequestObjectResult("Id is required");
                return;
            }

            var entity = _readContext.Set<T>().SingleOrDefault(x => x.Id.Equals(id));
            if (entity == null)
            {
                context.Result = new NotFoundResult();
            }
            //else
            //{
            //    context.HttpContext.Items.Add("entity", entity);
            //}
        }

    }
}
