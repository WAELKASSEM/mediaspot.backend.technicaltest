namespace Mediaspot.Application.Common;

public record PageSize
{
    const int defaultVal = 10;
    const int MaxSize = 100;
    public int Value { get; private set; }
    public PageSize(int? size)
    {
        if (!size.HasValue || size <= 0)
        {
            Value = defaultVal;
        }
        else
        {
            Value = Math.Min(size.Value, MaxSize);
        }
    }
}
