namespace Mediaspot.Application.Titles.Exceptions;

public class TitleAlreadyExistsException : Exception
{
    private TitleAlreadyExistsException(string message) : base(message)
    {
        
    }
    public static TitleAlreadyExistsException ForNameConflict(string titleName)
    {
        return new TitleAlreadyExistsException($"A title with the name '{titleName}' already exists.");
    }
}
