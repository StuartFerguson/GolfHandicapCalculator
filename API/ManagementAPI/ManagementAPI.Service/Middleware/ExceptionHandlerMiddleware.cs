using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Shared.General;
using Shared.EventStore;

namespace ManagementAPI.Service.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {                
                await HandleExceptionAsync(context, ex);                
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            Exception newException = new Exception($"An unhandled exception has occurred while executing the request. Url: {context.Request.GetDisplayUrl()}",exception);
            Logger.LogError(newException);
            
            // Set some defaults
            var response = context.Response;
            var statusCode = HttpStatusCode.InternalServerError;
            var message = "Unexpected error";

            if (exception is ArgumentException ||
                exception is InvalidOperationException ||
                exception is InvalidDataException ||
                exception is FormatException ||
                exception is NotSupportedException)
            {
                statusCode = HttpStatusCode.BadRequest;
                message = exception.Message;
            }
            else if (exception is NotFoundException)
            {
                statusCode = HttpStatusCode.NotFound;
                message = exception.Message;
            }
            else if (exception is NotImplementedException)
            {                
                statusCode = HttpStatusCode.NotImplemented;
                message = exception.Message;
            }

            response.ContentType = context.Request.ContentType;
            response.StatusCode = (Int32)statusCode;
            await response.WriteAsync(JsonConvert.SerializeObject(new ErrorResponse(message)));
        }

        [DataContract(Name= "ErrorResponse")]
        public class ErrorResponse
        {
            [DataMember(Name = "Message")]
            public String Message { get; set; }
            
            public ErrorResponse(String message)
            {
                Message = message;
            }
        }

    /*public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate next;
    private readonly IActionResultExecutor<ObjectResult> executor;
    private static readonly ActionDescriptor EmptyActionDescriptor = new ActionDescriptor();

    public ExceptionHandlerMiddleware(RequestDelegate next, IActionResultExecutor<ObjectResult> executor)
    {
        this.next = next;
        this.executor = executor;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            Exception newException =
                new Exception(
                    $"An unhandled exception has occurred while executing the request. Url: {context.Request.GetDisplayUrl()}. Request Data: " +
                    GetRequestData(context), ex);
            Logger.LogError(newException);

            if (context.Response.HasStarted)
            {
                throw;
            }

            var routeData = context.GetRouteData() ?? new RouteData();

            ClearCacheHeaders(context.Response);

            var actionContext = new ActionContext(context, routeData, EmptyActionDescriptor);

            var errorResponse = BuildErrorResponse(ex);

            await executor.ExecuteAsync(actionContext, errorResponse);
        }
    }

    private static String GetRequestData(HttpContext context)
    {
        var sb = new StringBuilder();

        if (context.Request.HasFormContentType && context.Request.Form.Any())
        {
            sb.Append("Form variables:");
            foreach (var x in context.Request.Form)
            {
                sb.AppendFormat("Key={0}, Value={1}<br/>", x.Key, x.Value);
            }
        }

        sb.AppendLine("Method: " + context.Request.Method);

        return sb.ToString();
    }

    private static void ClearCacheHeaders(HttpResponse response)
    {
        response.Headers[HeaderNames.CacheControl] = "no-cache";
        response.Headers[HeaderNames.Pragma] = "no-cache";
        response.Headers[HeaderNames.Expires] = "-1";
        response.Headers.Remove(HeaderNames.ETag);
    }

    private ObjectResult BuildErrorResponse(Exception exception)
    {
        ObjectResult result = null;

        if (exception is ArgumentException ||
            exception is InvalidOperationException ||
            exception is FormatException ||
            exception is NotSupportedException)
        {
            // Bad Request   
            result = new BadRequestObjectResult(exception.Message);
        }
        else if (exception is NotFoundException)
        {
            // Return a Not Implemented Error
            result = new ObjectResult(null)
            {
                StatusCode = (Int32)HttpStatusCode.NotFound,
                Value = exception.Message
            };
        }
        else if (exception is NotImplementedException)
        {                
            // Return a Not Implemented Error
            result = new ObjectResult(null)
            {
                StatusCode = (Int32)HttpStatusCode.NotImplemented,
                Value = exception.Message
            };
        }
        else
        {
            // Return an Internal Server Error
            result = new ObjectResult(null)
            {
                StatusCode = (Int32)HttpStatusCode.InternalServerError,
            };
        }

        return result;
    }

    [DataContract(Name= "ErrorResponse")]
    public class ErrorResponse
    {
        [DataMember(Name = "Message")]
        public String Message { get; set; }

        public ErrorResponse(String message)
        {
            Message = message;
        }
    }*/
}
}
