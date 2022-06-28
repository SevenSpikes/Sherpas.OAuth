using Sherpas.OAuth.Data.Entities;
using Sherpas.OAuth.Services.Database.Infrastructure;

namespace Sherpas.OAuth.Services.Database
{
    public interface IUserService: IDbService<User>
    {
        Task<User?> GetByUserNameAndCode(string userName, string password);
        Task<User?> GetByUserNameAndRefreshToken(string userName, string refreshToken);
        Task<User?> AddAuthenticationCodeForUser(string userName, string authenticationCode);
    }
}