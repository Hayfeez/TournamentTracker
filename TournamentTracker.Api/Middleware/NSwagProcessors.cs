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
    public class AddRequiredHeaderParameterOperationProcessor : IOperationProcessor
    {
        private string AccountId { get; set; }

        private string LoginId { get; set; }

        public AddRequiredHeaderParameterOperationProcessor(IConfiguration appSettings)
        {
            AccountId = appSettings["AccountId"].ToString();
            LoginId = appSettings["LoginId"].ToString(); 
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

    public class AddRequiredHeaderParameterDocumentProcessor : IDocumentProcessor
    {
        private string AccountId { get; set; }

        private string LoginId { get; set; }

        public AddRequiredHeaderParameterDocumentProcessor(IConfiguration appSettings)
        {
            AccountId = appSettings["AccountId"].ToString();
            LoginId = appSettings["LoginId"].ToString();
        }

        public void Process(DocumentProcessorContext context)
        {
            var operations = context.Document.Operations.ToList();
            foreach (var op in operations)
            {
                var operation = op.Operation;
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "LoginId",
                    IsRequired = true,
                    Default = LoginId,
                    Kind = OpenApiParameterKind.Header,
                    Type = JsonObjectType.String,
                    Description = "Login Id"

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

                //foreach (var pa in operation.Parameters)
                //{
                //    pa.Name = Regex.Replace(pa.Name, @"^\w*\.", "");
                //}
            }
        }
    }

}
