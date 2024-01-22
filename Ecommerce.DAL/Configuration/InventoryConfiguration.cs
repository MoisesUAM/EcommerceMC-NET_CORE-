using Ecommerce.Models.Catalog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.DAL.Configuration
{
    public class InventoryConfiguration : IEntityTypeConfiguration<InventoryModel>
    {
        public void Configure(EntityTypeBuilder<InventoryModel> builder)
        {
            builder.HasKey(x => x.IdInventory);
            builder.Property(x => x.IdStore).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x=>x.StartDate).IsRequired();
            builder.Property(x=>x.EndDate).IsRequired();
            builder.Property(x=>x.Estate).IsRequired();

            //Relaciones
            builder.HasOne(x=>x.Stores).WithMany()
                .HasForeignKey(x=>x.IdStore)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x=>x.Users).WithMany()
                .HasForeignKey(x=>x.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
