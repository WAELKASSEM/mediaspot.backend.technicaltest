namespace Mediaspot.Domain.Titles.ValueObjects;

public sealed class ReleaseDate
{
    public DateOnly? Value { get; }
    private ReleaseDate()
    {
    }
    public ReleaseDate(DateOnly? value)
    {
        if (!value.HasValue)
            return;

        Value = value; ;
    }
}

