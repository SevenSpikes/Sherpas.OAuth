namespace Sherpas.OAuth.Services.Application
{
    public interface IUserManager
    {
        Task RegisterAuthenticationCodeForUser(string userName, string authenticationCode);
    }
}