using Sherpas.OAuth.Services.Database;

namespace Sherpas.OAuth.Services.Application
{
    public class UserManager : IUserManager
    {
        private readonly IUserService _userService;

        public UserManager(IUserService userService)
        {
            _userService = userService;
        }
        public async Task RegisterAuthenticationCodeForUser(string userName, string authenticationCode)
        {
            await _userService.AddAuthenticationCodeForUser(userName, authenticationCode);
        }
    }
}
