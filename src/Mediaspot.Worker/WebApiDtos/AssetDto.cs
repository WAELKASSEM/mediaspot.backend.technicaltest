namespace Mediaspot.Worker.WebApiDtos;


public abstract record AssetDto
{
    public Guid Id { get; init;  }
    public string Type { get; init; } = string.Empty;
}
