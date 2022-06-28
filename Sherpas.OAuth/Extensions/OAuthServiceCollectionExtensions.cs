using System.Data;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Sherpas.OAuth.Data.Context;
using Sherpas.OAuth.Services.Application;
using Sherpas.OAuth.Services.Database;
using Sherpas.OAuth.Services.Database.Infrastructure;
using Sherpas.OAuth.Services.External;

namespace Sherpas.OAuth.Extensions
{
    public static class OAuthServiceCollectionExtensions
    {
        public static IServiceCollection AddOAuth(this IServiceCollection services,
            Action<OAuthOptions> options)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.Configure(options);

            var oAuthOptions = new OAuthOptions();
            options.Invoke(oAuthOptions);

            if (string.IsNullOrWhiteSpace(oAuthOptions.Secret))
            {
                throw new NoNullAllowedException("OAuthOptions.Secret");
            }

            services.AddAuthentication("SherpasOAuth")
                .AddJwtBearer("SherpasOAuth", jwt =>
                {
                    var secretBytes = Encoding.UTF8.GetBytes(oAuthOptions.Secret);

                    var key = new SymmetricSecurityKey(secretBytes);

                    jwt.Audience = oAuthOptions.Audience;
                    jwt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = oAuthOptions.Issuer,
                        ValidAudience = oAuthOptions.Audience,
                        IssuerSigningKey = key,
                    };
                });

            services.AddScoped(typeof(IDbService<>), typeof(BaseDbService<>));
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IUserManager, UserManager>();

            return services;
        }

        public static IServiceCollection UseSqlServer(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(connectionString));

            AuthDbContext authDbContext = services.BuildServiceProvider().GetService<AuthDbContext>();

            if (authDbContext.Database.GetPendingMigrations().Any())
            {
                authDbContext.Database.Migrate();
            }

            return services;
        }

        public static IServiceCollection UseClaimsManager<T>(this IServiceCollection services) where T : class, IClaimsManager
        {
            services.AddScoped<IClaimsManager, T>();

            return services;
        }
    }
}