using GrpcTraining.Resources.OpenDota;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;

namespace GrpcTraining.Clients
{
    public class OpenDotaClient : IOpenDotaClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly OpenDotaConfig _config;
        private readonly string? _apiKey;
        private readonly string? _apiBaseUrl;

        public OpenDotaClient(IOptions<OpenDotaConfig> _options, IHttpClientFactory httpClientFactory)
        {
            _config = _options.Value;
            _apiKey = _config.ApiKey;
            _apiBaseUrl = _config.BaseApiUrl;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IEnumerable<OpenDotaHero>> GetHeroes()
        {
            var client = _httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, _apiBaseUrl + "/heroes" + "?apiKey=" + _apiKey);

            var response = await client.SendAsync(request);

            response.EnsureSuccessStatusCode();

            using var contentStream = await response.Content.ReadAsStreamAsync();

            var heroes = await JsonSerializer.DeserializeAsync<IEnumerable<OpenDotaHero>>(contentStream);

            return heroes;

        }
    }
}
