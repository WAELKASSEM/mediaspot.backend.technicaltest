using Mediaspot.Api.Titles.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mediaspot.Api.Titles.ListTitles;

public static class ListTitlesEndpoint
{
    public static IEndpointRouteBuilder MapListTitlesEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("", ListTitlesAsync)
            .WithName("List Titles")
            .WithDescription("List Titles");

        return endpoints;
    }

    private static Task<IResult> ListTitlesAsync([FromServices] ISender store)
    {
        throw new NotImplementedException();
    }
}
