using Mediaspot.Api.Titles.GetTitleById;
using Mediaspot.Application.Titles.Commands.Update;
using Mediaspot.Domain.Titles.ValueObjects;

namespace Mediaspot.Api.Titles.UpdateTitle;

public record UpdateTitleDto(string? Name, TitleTypeDto? Type, string? Description, DateOnly? ReleaseDate);
public static class UpdateTitleDtoExtensions
{
    public static UpdateTitleCommand ToCommand(this UpdateTitleDto dto, Guid id)
    {
        return new UpdateTitleCommand(
            Id: id,
            Name: dto.Name,
            Type: dto.Type == null ? null : Enum.Parse<TitleType>(dto.Type.Value.ToString()),
            Description: dto.Description,
            ReleaseDate: dto.ReleaseDate);
    }
}