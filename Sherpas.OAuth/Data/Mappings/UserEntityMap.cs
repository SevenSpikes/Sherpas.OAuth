using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sherpas.OAuth.Data.Entities;

namespace Sherpas.OAuth.Data.Mappings
{
    public class UserEntityMap: IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Auth_User");
            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.TokenInfo).WithOne().HasForeignKey(x => x.UserId);
            builder.HasMany(x => x.AuthenticationCodes).WithOne().HasForeignKey(x => x.UserId);
        }
    }
}
