using System.Net.Http.Json;

namespace Mediaspot.Worker;

public sealed class MediaSpotApiClient(HttpClient httpClient)
{
    public async Task<T?> GetAsync<T>(string url, CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<T>(url, cancellationToken);
    }

    public async Task<HttpResponseMessage> PutAsync<T>(
        string url,
        T payload,
        CancellationToken cancellationToken = default)
    {
        return await httpClient.PutAsJsonAsync(url, payload, cancellationToken);
    }
    public async Task<HttpResponseMessage> PutAsync(
        string url,
        CancellationToken cancellationToken = default)
    {
        return await httpClient.PutAsync(url,null, cancellationToken);
    }
}
