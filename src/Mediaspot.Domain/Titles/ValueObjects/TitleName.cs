namespace Mediaspot.Domain.Titles.ValueObjects;

public sealed class TitleName
{
    public string Value { get; }
#pragma warning disable
    private TitleName()
    {
        
    }
#pragma warning enable
    public TitleName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Title name is required.", nameof(value));

        if (value.Length > 256)
            throw new ArgumentException("Title name is too long.", nameof(value));

        Value = value.Trim();
    }
}

