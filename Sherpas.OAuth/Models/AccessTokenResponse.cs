using Newtonsoft.Json;

namespace Sherpas.OAuth.Models
{
    public record AccessTokenResponse
    {
        [JsonProperty("client_id")]
        public string? ClientId { get; init; }

        [JsonProperty("access_token")]
        public string? AccessToken { get; init; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; init; }

        [JsonProperty("refresh_token")]
        public string? RefreshToken { get; init; }
    };
}