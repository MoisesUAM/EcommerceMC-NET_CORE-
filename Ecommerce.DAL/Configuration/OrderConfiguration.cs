using Ecommerce.Models.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.DAL.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<OrderModel>
    {
        public void Configure(EntityTypeBuilder<OrderModel> builder)
        {
            builder.HasKey(x => x.IdOrder);
            builder.Property(x => x.IdUser).IsRequired();
            builder.Property(x => x.ShippingDate).IsRequired();
            builder.Property(x => x.OrderDate).IsRequired();
            builder.Property(x => x.TrackingNumber).IsRequired(false);
            builder.Property(x => x.Carrier).IsRequired(false);
            builder.Property(x => x.TotalOrderAmount).IsRequired();
            builder.Property(x => x.OrderState).IsRequired();
            builder.Property(x => x.PaymentState).IsRequired();
            builder.Property(x => x.PaymentDate).IsRequired();
            builder.Property(x => x.IdTransaction).IsRequired(false);
            builder.Property(x => x.ShippingAddress).IsRequired(false);
            builder.Property(x => x.ClientName).IsRequired();
            builder.Property(x => x.City).IsRequired(false);
            builder.Property(x => x.Country).IsRequired(false);
            builder.Property(x => x.PhoneNumber).IsRequired(false);

            //navegacion

            builder.HasOne(x => x.Users).WithMany()
                .HasForeignKey(x => x.IdUser)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
