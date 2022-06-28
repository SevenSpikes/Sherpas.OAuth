namespace Sherpas.OAuth.Data.Entities
{
    public class TokenInfo
    {
        public int Id { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
    }
}
