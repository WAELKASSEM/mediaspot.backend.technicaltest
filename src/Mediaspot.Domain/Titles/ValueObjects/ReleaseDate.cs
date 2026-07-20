namespace Mediaspot.Domain.Titles.ValueObjects;

public sealed class ReleaseDate
{
    public DateTime Value { get; }

    public ReleaseDate(DateTime value)
    {
        if (value < new DateTime(1900, 1, 1))
            throw new ArgumentException("Release date is too early.", nameof(value));

        if (value > DateTime.UtcNow.AddYears(5))
            throw new ArgumentException("Release date is unrealistically far in the future.", nameof(value));

        Value = value.Date;
    }
}

