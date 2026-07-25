using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mediaspot.Worker.WebApiDtos;

public class AssetDtoConverter : JsonConverter<AssetDto>
{
    public override AssetDto Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Parse into JsonDocument for inspection
        using (var jsonDoc = JsonDocument.ParseValue(ref reader))
        {
            var root = jsonDoc.RootElement;

            if (!root.TryGetProperty("type", out var typeProp))
                throw new JsonException("Missing Type discriminator in JSON.");

            string type = typeProp.GetString()!;

            return type switch
            {
                "Video" => JsonSerializer.Deserialize<VideoAssetDto>(root.GetRawText(), options)!,
                "Audio" => JsonSerializer.Deserialize<AudioAssetDto>(root.GetRawText(), options)!,
                _ => throw new JsonException($"Unknown asset type: {type}")
            };
        }
    }

    public override void Write(Utf8JsonWriter writer, AssetDto value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}