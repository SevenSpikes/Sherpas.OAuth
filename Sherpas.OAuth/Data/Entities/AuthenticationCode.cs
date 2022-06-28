namespace Sherpas.OAuth.Data.Entities
{
    public class AuthenticationCode
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public bool Consumed { get; set; }
        public int UserId { get; set; }
    }
}
