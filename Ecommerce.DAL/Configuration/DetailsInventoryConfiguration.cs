using Ecommerce.Models.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.DAL.Configuration
{
    public class DetailsInventoryConfiguration : IEntityTypeConfiguration<DetailsInventoryModels>
    {
        public void Configure(EntityTypeBuilder<DetailsInventoryModels> builder)
        {
            builder.HasKey(x => x.IdDetailsIventory);
            builder.Property(x => x.LastStock).IsRequired();
            builder.Property(x => x.Quantity).IsRequired();
            builder.Property(x => x.IdInventory).IsRequired();
            builder.Property(x => x.IdProducto).IsRequired();

            //Navegaciones o relaciones

            builder.HasOne(x => x.Inventory).WithMany()
                .HasForeignKey(x => x.IdInventory).
                OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x=> x.Product).WithMany()
                .HasForeignKey(x=>x.IdProducto)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
