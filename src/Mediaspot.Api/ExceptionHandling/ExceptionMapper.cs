using Mediaspot.Application.Common.Exceptions;
using Mediaspot.Application.Titles.Exceptions;
using System.Net;

namespace Mediaspot.Api.ExceptionHandling;

public static class ExceptionMapper
{
    public static HttpStatusCode MapToStatusCode(Exception exception)
    {
        return exception switch
        {
            TitleAlreadyExistsException => HttpStatusCode.BadRequest,
            EntityNotFoundException => HttpStatusCode.NotFound,
            _ => HttpStatusCode.InternalServerError
        };
    }
}
