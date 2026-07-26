namespace Mediaspot.Api.Titles;

using Mediaspot.Api.Titles.CreateTitle;
using Mediaspot.Api.Titles.UpdateTitle;
using Mediaspot.Api.Titles.GetTitleById;
using Mediaspot.Api.Titles.ListTitles;

public static class EndpointsGroup
{
    public static IEndpointRouteBuilder MapTitlesEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/titles").WithTags("Titles");

        group.MapCreateTitleEndpoint();
        group.MapUpdateTitleEndpoint();
        group.MapGetTitleByIdEndpoint();
        group.MapListTitlesEndpoint();

        return endpoints;
    }
}
