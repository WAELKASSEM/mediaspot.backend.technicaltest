using Mediaspot.Application.Titles.Commands.Update;
using Mediaspot.Domain.Titles.ValueObjects;

namespace Mediaspot.Api.Titles.UpdateTitle;

public record UpdateTitleDto(string? Name, TitleType? Type, string? Description, DateTime? ReleaseDate);
public static class UpdateTitleDtoExtensions
{
    public static UpdateTitleCommand ToCommand(this UpdateTitleDto dto, Guid id)
    {
        return new UpdateTitleCommand(
            Id: id,
            Name: dto.Name,
            Type: dto.Type,
            Description: dto.Description,
            ReleaseDate: dto.ReleaseDate.HasValue ? DateOnly.FromDateTime(dto.ReleaseDate.Value) : null);
    }
}