using Ecommerce.Models.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.DAL.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<UserModel>
    {
        public void Configure(EntityTypeBuilder<UserModel> builder)
        {
            builder.Property(u=>u.Name).IsRequired().HasMaxLength(100);
            builder.Property(u=>u.LastName).IsRequired().HasMaxLength(100);
            builder.Property(u=>u.Address).IsRequired().HasMaxLength(200);
            builder.Property(u=>u.City).IsRequired().HasMaxLength(100);
            builder.Property(u=>u.Country).IsRequired().HasMaxLength(100);
        }
    }
}
