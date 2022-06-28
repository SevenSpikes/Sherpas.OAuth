using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sherpas.OAuth.Data.Entities;

namespace Sherpas.OAuth.Data.Mappings
{
    public class TokenInfoEntityMap: IEntityTypeConfiguration<TokenInfo>
    {
        public void Configure(EntityTypeBuilder<TokenInfo> builder)
        {
            builder.ToTable("Auth_TokenInfo");
            builder.HasKey(x => x.Id);
        }
    }
}
