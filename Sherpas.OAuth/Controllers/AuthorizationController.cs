using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sherpas.OAuth.Data.Entities;
using Sherpas.OAuth.Extensions;
using Sherpas.OAuth.Models;
using Sherpas.OAuth.Services.Database;
using Sherpas.OAuth.Services.External;

namespace Sherpas.OAuth.Controllers
{
    [ApiController]
    [Route("connect")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IServiceProvider _serviceProvider;
        private readonly OAuthOptions _oAuthOptions;

        public AuthorizationController(IUserService userService, IServiceProvider serviceProvider,
            IOptions<OAuthOptions> options)
        {
            _userService = userService;
            _serviceProvider = serviceProvider;
            _oAuthOptions = options.Value;
        }

        [HttpPost]
        [Route("token")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> IssueToken(
            [FromForm] string grant_type,
            [FromForm] string client_id,
            [FromForm] string? code,
            [FromForm] string? refresh_token)
        {
            AccessTokenResponse? accessTokenResponse = null;

            switch (grant_type)
            {
                case "code":
                    accessTokenResponse = await ProcessCodeGrant(client_id, code);
                    break;
                case "refresh_token":
                    accessTokenResponse = await ProcessRefreshGrant(client_id, refresh_token);
                    break;
            }

            if (accessTokenResponse != null)
            {
                return Ok(accessTokenResponse);
            }

            return BadRequest($"Invalid grant_type {grant_type}");
        }

        private async Task<AccessTokenResponse?> ProcessRefreshGrant(string userName, string? refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                return null;

            var user = await _userService.GetByUserNameAndRefreshToken(userName, refreshToken);

            if (user == null)
                return null;

            // Invalidate the refresh token
            var ti = user.TokenInfo.FirstOrDefault(x => x.RefreshToken == refreshToken);

            if (ti != null)
            {
                user.TokenInfo.Remove(ti);
                await _userService.Update(user);
            }

            return await GenerateTokenResponse(user);
        }

        private async Task<AccessTokenResponse?> ProcessCodeGrant(string clientId, string? code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return null;

            var user = await _userService.GetByUserNameAndCode(clientId, code);

            if (user == null)
                return null;

            // Invalidate the access code
            var ac = user.AuthenticationCodes.FirstOrDefault(x => x.Code == code);

            if (ac != null)
            {
                ac.Consumed = true;
                await _userService.Update(user);
            }

            return await GenerateTokenResponse(user);
        }

        private async Task<AccessTokenResponse> GenerateTokenResponse(User user)
        {
            //If the application which uses this Authentication assembly does not provide a IClaimsManager,
            //we use the following default claims.
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.NameIdentifier, user.UserName)
            };

            var claimsManager = _serviceProvider.GetService<IClaimsManager>();

            if (claimsManager != null)
            {
                var claimsDictionary = await claimsManager.GetClaimsForClient(user.UserName);
                claims = claimsDictionary.Select(x => new Claim(x.Key, x.Value)).ToList();
            }

            var secretBytes = Encoding.UTF8.GetBytes(_oAuthOptions.Secret!);

            var key = new SymmetricSecurityKey(secretBytes);

            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var accessTokenExpirationTimespan = TimeSpan.FromSeconds(_oAuthOptions.AccessTokenExpirationSeconds);

            var token = new JwtSecurityToken(
                issuer: _oAuthOptions.Issuer,
                audience: _oAuthOptions.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.Add(accessTokenExpirationTimespan),
                signingCredentials: signingCredentials
            );

            var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = GenerateRefreshToken();

            user.TokenInfo.Add(new TokenInfo
            {
                AccessToken = tokenJson,
                RefreshToken = refreshToken
            });

            await _userService.Update(user);

            return new AccessTokenResponse
            {
                AccessToken = tokenJson,
                RefreshToken = refreshToken,
                ExpiresIn = accessTokenExpirationTimespan.Seconds
            };
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();

            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
