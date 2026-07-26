namespace Mediaspot.IntegrationTests.Titles;

using Mediaspot.Api.Titles.CreateTitle;
using Mediaspot.Api.Titles.GetTitleById;
using Mediaspot.Api.Titles.UpdateTitle;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;
using static AspireSingletonFeature;

[TestFixture]
public class TitleCrudTests
{
    static CreateTitleDto BuildRandom()
    {
        return new CreateTitleDto(Guid.NewGuid().ToString(), TitleTypeDto.Movie, Guid.NewGuid().ToString(), DateOnly.MaxValue);
    }

    [Test]
    public async Task CreateTitle_HappyPath()
    {
        CreateTitleDto titleDto = BuildRandom();

        var createdResponse = await ApiClient.PostAsJsonAsync("titles", titleDto);
        var createdId = await createdResponse.Content.ReadFromJsonAsync<Guid>();
        var getResponse = await ApiClient.GetAsync($"titles/{createdId}");
        var getDto = await getResponse.Content.ReadFromJsonAsync<TitleDto>();

        Assert.Multiple(() =>
        {
            Assert.That(getDto!.ReleaseDate, Is.EqualTo(titleDto.ReleaseDate));
            Assert.That(getDto!.Type.ToString(), Is.EqualTo(titleDto.Type.ToString()));
            Assert.That(getDto!.Description, Is.EqualTo(titleDto.Description));
            Assert.That(getDto!.Name, Is.EqualTo(titleDto.Name));
        });

    }
    [Test]
    public async Task CreateTitle_WithNullableFields_HappyPath()
    {
        CreateTitleDto titleDto = new(Guid.NewGuid().ToString(), TitleTypeDto.Other, null, null);


        var createdResponse = await ApiClient.PostAsJsonAsync("titles", titleDto);
        var createdId = await createdResponse.Content.ReadFromJsonAsync<Guid>();
        var getResponse = await ApiClient.GetAsync($"titles/{createdId}");
        var getDto = await getResponse.Content.ReadFromJsonAsync<TitleDto>();

        Assert.Multiple(() =>
        {
            Assert.That(getDto!.ReleaseDate, Is.Null);
            Assert.That(getDto!.Type.ToString(), Is.EqualTo(titleDto.Type.ToString()));
            Assert.That(getDto!.Description, Is.Null);
            Assert.That(getDto!.Name, Is.EqualTo(titleDto.Name));
        });
    }

    [Test]
    public async Task UpdateTitle_HappyPath()
    {
        CreateTitleDto titleDto = BuildRandom();
        var createdResponse = await ApiClient.PostAsJsonAsync("titles", titleDto);
        var createdId = await createdResponse.Content.ReadFromJsonAsync<Guid>();

        UpdateTitleDto updateDto = new UpdateTitleDto(Guid.NewGuid().ToString(), TitleTypeDto.Documentary, Guid.NewGuid().ToString(), DateOnly.MinValue);
        var updatedResponse = await ApiClient.PutAsJsonAsync($"titles/{createdId}", updateDto);
        var getResponse = await ApiClient.GetAsync($"titles/{createdId}");
        var updatedDto = await getResponse.Content.ReadFromJsonAsync<TitleDto>();

        Assert.Multiple(() =>
        {
            Assert.That(updatedDto!.ReleaseDate, Is.EqualTo(updateDto.ReleaseDate));
            Assert.That(updatedDto!.Type.ToString(), Is.EqualTo(updateDto.Type.ToString()));
            Assert.That(updatedDto!.Description, Is.EqualTo(updateDto.Description));
            Assert.That(updatedDto!.Name, Is.EqualTo(updateDto.Name));
        });
    }
    [Test]
    public async Task UpdateTitle_KeepOriginalProperties_HappyPath()
    {
        CreateTitleDto titleDto = BuildRandom();
        var createdResponse = await ApiClient.PostAsJsonAsync("titles", titleDto);
        var createdId = await createdResponse.Content.ReadFromJsonAsync<Guid>();

        UpdateTitleDto updateDto = new UpdateTitleDto(null, null, null, null);
        var updatedResponse = await ApiClient.PutAsJsonAsync($"titles/{createdId}", updateDto);
        var getResponse = await ApiClient.GetAsync($"titles/{createdId}");
        var updatedDto = await getResponse.Content.ReadFromJsonAsync<TitleDto>();

        Assert.Multiple(() =>
        {
            Assert.That(updatedDto!.ReleaseDate, Is.EqualTo(titleDto.ReleaseDate));
            Assert.That(updatedDto!.Type.ToString(), Is.EqualTo(titleDto.Type.ToString()));
            Assert.That(updatedDto!.Description, Is.EqualTo(titleDto.Description));
            Assert.That(updatedDto!.Name, Is.EqualTo(titleDto.Name));
        });
    }
    [Test]
    public async Task CreateTitle_SameName_ThrowsException()
    {
        CreateTitleDto titleDto = BuildRandom();

        await ApiClient.PostAsJsonAsync("titles", titleDto);

        HttpResponseMessage response =
            await ApiClient.PostAsJsonAsync("titles", titleDto with { Name = titleDto.Name.ToUpper() }); //on test la collation aussi.

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));

        ProblemDetails? problem =
            await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.Multiple(() =>
        {
            Assert.That(problem, Is.Not.Null);
            Assert.That(problem!.Status, Is.EqualTo(409));
        });

    }


    [Test]
    public async Task ListTitles_UsingCursor_ReturnsRemainingTitles()
    {
        var title1 = BuildRandom();
        var title2 = BuildRandom();
        var title3 = BuildRandom();

        await ApiClient.PostAsJsonAsync("titles", title1);
        await ApiClient.PostAsJsonAsync("titles", title2);
        await ApiClient.PostAsJsonAsync("titles", title3);

        var page1Response = await ApiClient.GetFromJsonAsync<List<TitleDto>>(
            "titles?pageSize=2");

        Assert.That(page1Response, Is.Not.Null);
        Assert.That(page1Response, Has.Count.EqualTo(2));

        var cursor = page1Response.Last().Id;

        var page2Response = await ApiClient.GetFromJsonAsync<List<TitleDto>>(
            $"titles?pageSize=2&lastSeen={cursor}");

        Assert.That(page2Response, Is.Not.Null);
        Assert.That(page2Response.Select(x => x.Id).All(y => y > page1Response.Last().Id), Is.True);
    }

}
