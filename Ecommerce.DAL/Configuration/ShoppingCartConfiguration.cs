using Ecommerce.Models.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.DAL.Configuration
{
    public class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCartModel>
    {
        public void Configure(EntityTypeBuilder<ShoppingCartModel> builder)
        {
            builder.HasKey(x => x.IdShoppingCart);
            builder.Property(x=>x.UserId).IsRequired();
            builder.Property(x=>x.IdProduct).IsRequired();
            builder.Property(x=>x.Quantity).IsRequired();

            //Relaciones
            builder.HasOne(x=>x.Users).WithMany()
                .HasForeignKey(x=>x.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x=>x.Products).WithMany()
                .HasForeignKey(x=>x.IdProduct)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
