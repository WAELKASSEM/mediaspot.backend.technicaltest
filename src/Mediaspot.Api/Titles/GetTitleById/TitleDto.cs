namespace Mediaspot.Api.Titles.GetTitleById;

public record TitleDto(Guid Id, string Name, string Type, string? Description, DateOnly? ReleaseDate);
public static class TitleDtoExtensions
{
    public static TitleDto ToDto(this Domain.Titles.Title title)
    {
        return new TitleDto(title.Id, title.Name.Value, title.Type.ToString(), title.Description.Value, title.ReleaseDate.Value);
    }
}