using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace AuthenticationTest.Services
{
    public class GraphAuth
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public GraphAuth(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var tenantId = _configuration["MicrosoftGraph:TenantId"];
            var clientId = _configuration["MicrosoftGraph:ClientId"];
            var clientSecret = _configuration["MicrosoftGraph:ClientSecret"];

            var tokenUrl = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token";
            var requestBody = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("scope", "https://graph.microsoft.com/.default"),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
            });

            var response = await _httpClient.PostAsync(tokenUrl, requestBody);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to retrieve access token: {responseBody}");
            }

            var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseBody);
            return jsonResponse.GetProperty("access_token").GetString();
        }
    }
}
