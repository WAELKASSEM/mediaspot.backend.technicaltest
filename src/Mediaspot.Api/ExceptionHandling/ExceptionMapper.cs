using Mediaspot.Application.Common.Exceptions;
using Mediaspot.Application.Titles.Exceptions;
using Mediaspot.Domain.Transcoding.Exceptions;
using System.Net;

namespace Mediaspot.Api.ExceptionHandling;

public static class ExceptionMapper
{
    public static HttpStatusCode MapToStatusCode(Exception exception)
    {
        return exception switch
        {
            TitleAlreadyExistsException => HttpStatusCode.Conflict,
            EntityNotFoundException => HttpStatusCode.NotFound,
            InvalidTranscodeStatusException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };
    }
}
