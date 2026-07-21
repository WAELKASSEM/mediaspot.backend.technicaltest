using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Mediaspot.Api.ExceptionHandling;

public class GlobalExceptionHandler(IHostEnvironment hostEnvironment) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        HttpStatusCode statusCode = ExceptionMapper.MapToStatusCode(exception);

        ProblemDetails problemDetails = new()
        {
            Status = (int)statusCode,
            Title = exception.Message,
            Instance = httpContext.Request.Path,
            Type = $"https://httpstatuses.com/{(int)statusCode}",

        };
        if (hostEnvironment.IsDevelopment())
        {
            problemDetails.Detail = exception.StackTrace;
        }


        httpContext.Response.StatusCode = problemDetails.Status.Value;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}