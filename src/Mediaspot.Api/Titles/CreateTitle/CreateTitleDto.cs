using Mediaspot.Application.Titles.Commands.Create;
using Mediaspot.Domain.Titles.ValueObjects;

namespace Mediaspot.Api.Titles.CreateTitle;

public record CreateTitleDto(string Name, TitleType Type, string? Description, DateOnly? ReleaseDate);

public static class CreateTitleDtoExtensions
{
    public static CreateTitleCommand ToCommand(this CreateTitleDto dto)
    {
        return new CreateTitleCommand(
            Name: dto.Name,
            Type: dto.Type,
            Description: dto.Description,
            ReleaseDate: dto.ReleaseDate);
    }
}
