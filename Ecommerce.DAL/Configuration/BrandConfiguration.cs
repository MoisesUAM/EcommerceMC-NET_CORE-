using Ecommerce.Models.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.DAL.Configuration
{
    public class BrandConfiguration : IEntityTypeConfiguration<BrandModel>
    {
        public void Configure(EntityTypeBuilder<BrandModel> builder)
        {
            builder.Property(b=>b.IdBrand).IsRequired();
            builder.Property(b=>b.Name).IsRequired().HasMaxLength(60);
            builder.Property(b=>b.Description).IsRequired().HasMaxLength(100);
            builder.Property(b => b.Estate).IsRequired();
        }
    }
}
