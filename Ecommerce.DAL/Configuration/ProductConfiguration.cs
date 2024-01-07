using Ecommerce.Models.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.DAL.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<ProductModel>
    {
        public void Configure(EntityTypeBuilder<ProductModel> builder)
        {
            builder.Property(p=>p.IdProduct).IsRequired();
            builder.Property(p=>p.SerialNumber).IsRequired().HasMaxLength(60);
            builder.Property(p=>p.Description).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Estate).IsRequired();
            builder.Property(p=>p.Price).IsRequired();
            builder.Property(p=>p.CostPrice).IsRequired();
            builder.Property(p=>p.IdCategory).IsRequired();
            builder.Property(p=>p.IdBrand).IsRequired();
            builder.Property(p=>p.ImageUrl).IsRequired(false);
            builder.Property(p=>p.MasterId).IsRequired(false);

            /* Relaciones*/
            builder.HasOne(x=>x.Category).WithMany()
                .HasForeignKey(x => x.IdCategory)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x=>x.Brand).WithMany()
                .HasForeignKey(x=>x.IdBrand)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x=>x.Master).WithMany()
                .HasForeignKey(x=>x.MasterId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
