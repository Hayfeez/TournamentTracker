using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using Newtonsoft.Json;

using TournamentTracker.Data.Interfaces;

namespace TournamentTracker.Api.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //can't do this since model for create and update are dtos not the ientity

            //var param = context.ActionArguments.SingleOrDefault(p => p.Value is IEntity);
            //if (param.Value == null)
            //{
            //    context.Result = new BadRequestObjectResult("Object is null");
            //    return;
            //}

            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }

            base.OnActionExecuting(context);
        }

    }
}
