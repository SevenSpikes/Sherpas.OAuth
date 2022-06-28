using Microsoft.EntityFrameworkCore;
using Sherpas.OAuth.Data.Context;
using Sherpas.OAuth.Data.Entities;
using Sherpas.OAuth.Services.Database.Infrastructure;

namespace Sherpas.OAuth.Services.Database
{
    internal class UserService : BaseDbService<User>, IUserService
    {
        private const int AuthenticationCodeValidMinutes = 2;

        public UserService(AuthDbContext authDbContext) : base(authDbContext)
        {
        }

        public async Task<User?> AddAuthenticationCodeForUser(string userName, string authenticationCode)
        {
            var user = await Entities
                .Include(x => x.AuthenticationCodes)
                .FirstOrDefaultAsync(x => x.UserName == userName);

            if (user == null)
            {
                user = new User
                {
                    UserName = userName,
                    CreatedAtUtc = DateTime.UtcNow,
                    IsActive = true,
                    AuthenticationCodes = new List<AuthenticationCode>
                    {
                        new()
                        {
                            Code = authenticationCode,
                            CreatedAtUtc = DateTime.UtcNow
                        }
                    }
                };

                await Entities.AddAsync(user);
            }
            else
            {
                var expiredAuthenticationCodes = user.AuthenticationCodes
                        .Where(x => !x.Consumed && x.CreatedAtUtc.AddMinutes(AuthenticationCodeValidMinutes) < DateTime.UtcNow);

                foreach (var previousCode in expiredAuthenticationCodes)
                {
                    previousCode.Consumed = true;
                }

                user.AuthenticationCodes.Add(new AuthenticationCode
                {
                    Code = authenticationCode,
                    CreatedAtUtc = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User?> GetByUserNameAndRefreshToken(string userName, string refreshToken)
        {
            return await Entities
                .Include(x => x.TokenInfo)
                .FirstOrDefaultAsync(x => x.UserName == userName && x.TokenInfo.Any(t => t.RefreshToken == refreshToken));
        }

        public async Task<User?> GetByUserNameAndCode(string userName, string code)
        {
            return await Entities
                .Include(x => x.TokenInfo)
                .Include(x => x.AuthenticationCodes)
                .FirstOrDefaultAsync(x => x.UserName == userName &&
                                          x.AuthenticationCodes.Any(authCode => authCode.Code == code && !authCode.Consumed));
        }
    }
}
