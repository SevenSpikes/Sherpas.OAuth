using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sherpas.OAuth.Data.Entities;

namespace Sherpas.OAuth.Data.Mappings
{
    public class AuthenticationCodeMap: IEntityTypeConfiguration<AuthenticationCode>
    {
        public void Configure(EntityTypeBuilder<AuthenticationCode> builder)
        {
            builder.ToTable("Auth_AuthenticationCode");
            builder.HasKey(x => x.Id);
        }
    }
}
