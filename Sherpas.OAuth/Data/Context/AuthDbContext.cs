using Microsoft.EntityFrameworkCore;
using Sherpas.OAuth.Data.Mappings;

namespace Sherpas.OAuth.Data.Context
{
    public class AuthDbContext: DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options): base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityMap());
            modelBuilder.ApplyConfiguration(new TokenInfoEntityMap());
            modelBuilder.ApplyConfiguration(new AuthenticationCodeMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}
