using Netscrape.Services.Interfaces;

namespace Netscrape.Services.Implementations;

public class HttpClientFacade : IHttpClientFacade
{
    private readonly HttpClient _httpClient;

    public HttpClientFacade(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<string> GetAsync(string uri)
    {
        var response = await _httpClient.GetAsync(uri);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}