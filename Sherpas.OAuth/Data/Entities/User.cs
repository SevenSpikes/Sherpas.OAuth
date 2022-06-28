namespace Sherpas.OAuth.Data.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public bool IsActive { get; set; }

        public List<TokenInfo> TokenInfo { get; set; }

        public List<AuthenticationCode> AuthenticationCodes { get; set; }
    }
}
