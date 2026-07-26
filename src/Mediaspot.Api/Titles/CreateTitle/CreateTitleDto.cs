using Mediaspot.Api.Titles.GetTitleById;
using Mediaspot.Application.Titles.Commands.Create;
using Mediaspot.Domain.Titles.ValueObjects;

namespace Mediaspot.Api.Titles.CreateTitle;

public record CreateTitleDto(string Name, TitleTypeDto Type, string? Description, DateOnly? ReleaseDate);

public static class CreateTitleDtoExtensions
{
    public static CreateTitleCommand ToCommand(this CreateTitleDto dto)
    {
        return new CreateTitleCommand(
            Name: dto.Name,
            Type: Enum.Parse<TitleType>(dto.Type.ToString()),
            Description: dto.Description,
            ReleaseDate: dto.ReleaseDate);
    }
}
