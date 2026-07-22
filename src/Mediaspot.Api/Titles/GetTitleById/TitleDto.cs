namespace Mediaspot.Api.Titles.GetTitleById;

public enum TitleTypeDto
{
    Movie,
    Series,
    Documentary,
    Short,
    Other
}
public record TitleDto(Guid Id, string Name, TitleTypeDto Type, string? Description, DateOnly? ReleaseDate);
public static class TitleDtoExtensions
{
    public static TitleDto ToDto(this Domain.Titles.Title title)
    {
        return new TitleDto(title.Id, title.Name.Value, Enum.Parse<TitleTypeDto>(title.Type.ToString()), title.Description?.Value, title.ReleaseDate?.Value);
    }
}