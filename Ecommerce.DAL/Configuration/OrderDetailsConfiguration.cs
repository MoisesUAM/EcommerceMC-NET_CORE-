using Ecommerce.Models.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.DAL.Configuration
{
    public class OrderDetailsConfiguration : IEntityTypeConfiguration<OrderDetailsModel>
    {
        public void Configure(EntityTypeBuilder<OrderDetailsModel> builder)
        {
            builder.HasKey(x => x.IdOrderDetail);
            builder.Property(x => x.IdOrder).IsRequired();
            builder.Property(x => x.IdProduct).IsRequired();
            builder.Property(x => x.Quantity).IsRequired();
            builder.Property(x => x.Price).IsRequired();

            //Navegaciones

            builder.HasOne(x=>x.Orders).WithMany()
                .HasForeignKey(x=>x.IdOrder)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x=>x.Products).WithMany()
                .HasForeignKey(x=> x.IdProduct)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
