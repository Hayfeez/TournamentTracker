using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;

using Namotion.Reflection;

using Newtonsoft.Json;

using NJsonSchema;
using NJsonSchema.Generation;

using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using TournamentTracker.Common.Attributes;

namespace TournamentTracker.Api.Middleware
{
    public class AddRequiredHeaderParameter : IOperationProcessor
    {
        private string AccountId { get; set; }

        private string LoginId { get; set; }

        public AddRequiredHeaderParameter(IConfiguration appSettings)
        {
            AccountId = "account id";
            LoginId = "login id";
        }
        public bool Process(OperationProcessorContext context)
        {
            context.OperationDescription.Operation.Parameters.Add(
                new OpenApiParameter
                {
                    Name = "Authorization",
                    IsRequired = true,
                    Default = LoginId,
                    Kind = OpenApiParameterKind.Header,
                    Type = JsonObjectType.String,
                    Description = "Access Token"
                });

            context.OperationDescription.Operation.Parameters.Add(
                new OpenApiParameter
                {
                    Name = "AccountId",
                    IsRequired = false,
                    Default = AccountId,
                    Kind = OpenApiParameterKind.Header,
                    Type = JsonObjectType.String,
                    Description = "Account Id"
                });

            return true;
        }
    }

    public class JsonIgnoreDocumentProcessor : IDocumentProcessor
    {
        public void Process(DocumentProcessorContext context)
        {
            var operations = context.Document.Operations.ToList();
            foreach (var op in operations)
            {
                var operation = op.Operation;
                var paramProperties  = operation.Parameters
                    .Select(p => p.Properties).ToList();

                       // .Where(prop => prop..GetCustomAttribute<JsonIgnoreAttribute>() != null);

                //operation.Parameters.Add(new OpenApiParameter
                //{
                //    Name = "Authorization",
                //    IsRequired = true,
                //    Default = LoginId,
                //    Kind = OpenApiParameterKind.Header,
                //    Type = JsonObjectType.String,
                //    Description = "Access Token"

                //});
            }
        }
    }

    public class DefaultDocumentProcessor : IDocumentProcessor
    {
        private string AccountId { get; set; }

        private string LoginId { get; set; }

        public DefaultDocumentProcessor(IConfiguration appSettings)
        {
            AccountId = "account id";
            LoginId = "login id";
        }

        public void Process(DocumentProcessorContext context)
        {
            var operations = context.Document.Operations.ToList();
            foreach (var op in operations)
            {
                var operation = op.Operation;
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "Authorization",
                    IsRequired = true,
                    Default = LoginId,
                    Kind = OpenApiParameterKind.Header,
                    Type = JsonObjectType.String,
                    Description = "Access Token"

                });

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "AccountId",
                    IsRequired = false,
                    Default = AccountId,
                    Kind = OpenApiParameterKind.Header,
                    Type = JsonObjectType.String,
                    Description = "Account Id"

                });

                foreach (var pa in operation.Parameters)
                {
                    pa.Name = Regex.Replace(pa.Name, @"^\w*\.", "");
                }
            }
        }
    }

    public class JsonIgnoreProcessor : IOperationProcessor
    {
        public bool Process(OperationProcessorContext context)
        {
            var ignoredProperties = context.MethodInfo.GetParameters()
                .SelectMany(p => p.ParameterType.GetProperties()
                    .Where(prop => prop.GetCustomAttribute<JsonIgnoreAttribute>() != null));

            if (ignoredProperties.Any())
            {
                foreach (var property in ignoredProperties)
                {
                    var param = context.OperationDescription.Operation.Parameters.SingleOrDefault(p => p.Name.Equals(property.Name, StringComparison.InvariantCulture));
                    if (param != null)
                    {
                        context.OperationDescription.Operation.Parameters.Remove(param);
                    }
                   
                }
            }

            return true;
        }
    }
}
