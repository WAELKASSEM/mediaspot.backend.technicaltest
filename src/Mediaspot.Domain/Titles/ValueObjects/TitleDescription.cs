namespace Mediaspot.Domain.Titles.ValueObjects;

public sealed class TitleDescription
{
    public string? Value { get; }

    private TitleDescription()
    {
    }

    public TitleDescription(string? value)
    {
        if (value is { Length: > 2000 })
            throw new ArgumentException("Title description is too long.", nameof(value));

        Value = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}

