namespace Mediaspot.Domain.Assets.VideoAssets.ValueObjects;


public sealed record Resolution
{
    private Resolution(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public int Width { get; }

    public int Height { get; }

    public static Resolution Create(int width, int height)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width,nameof(width));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height,nameof(height));

        return new Resolution(width, height);
    }

    public static Resolution HD => new(1280, 720);

    public static Resolution FHD => new(1920, 1080);

    public static Resolution QHD => new(2560, 1440);

    public static Resolution UHD4K => new(3840, 2160);

    public override string ToString()
        => $"{Width}x{Height}";
}

