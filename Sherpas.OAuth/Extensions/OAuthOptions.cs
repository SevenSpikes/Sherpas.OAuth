namespace Sherpas.OAuth.Extensions
{
    public class OAuthOptions
    {
        public string Issuer { get; set; } = "api1";
        public string Audience { get; set; } = "api1";
        public string? Secret { get; set; }
        public int AccessTokenExpirationSeconds { get; set; } = 120;
    }
}
