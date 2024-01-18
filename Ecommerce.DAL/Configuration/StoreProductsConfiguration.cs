using Ecommerce.Models.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.DAL.Configuration
{
    public class StoreProductsConfiguration : IEntityTypeConfiguration<StoreProductModel>
    {
        public void Configure(EntityTypeBuilder<StoreProductModel> builder)
        {
            //Se establecen ambas llaves foranes como primary key de la tabla intermedia
            builder.HasKey(x=> new {x.IdProduct, x.IdStore}).IsClustered();
            builder.Property(x=>x.IdStore).IsRequired();
            builder.Property(x=>x.IdProduct).IsRequired();
            builder.Property(x => x.OnHand);

            //Realaciones

            builder.HasOne(x=>x.Stores).WithMany()
                .HasForeignKey(x=>x.IdStore)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Products).WithMany()
                .HasForeignKey(x=>x.IdProduct)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
