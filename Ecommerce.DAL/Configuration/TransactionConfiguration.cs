using Ecommerce.Models.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.DAL.Configuration
{
    public class TransactionConfiguration : IEntityTypeConfiguration<TransactionsModel>
    {
        public void Configure(EntityTypeBuilder<TransactionsModel> builder)
        {
            builder.HasKey(x => x.IdTransaction);
            builder.Property(x=>x.IdStore).IsRequired();
            builder.Property(x=>x.IdProduct).IsRequired();
            builder.Property(x=>x.UserId).IsRequired();
            builder.Property(x=>x.Type).IsRequired().HasMaxLength(100);
            builder.Property(x=>x.Comments).IsRequired();
            builder.Property(x=>x.LastStock).IsRequired();
            builder.Property(x=>x.CurrentStock).IsRequired();
            builder.Property(x=>x.Quantity).IsRequired();
            builder.Property(x=>x.Cost).IsRequired();
            //Forma de insertar una columna calculada con EntityFrameWork
            builder.Property(x=>x.CostAmount).HasComputedColumnSql("[Quantity] * [Cost]");
            builder.Property(x=>x.CommitDate).IsRequired();

            //Navegaciones
            builder.HasOne(x=>x.User).WithMany()
                .HasForeignKey(x=>x.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x=>x.StoreProduct).WithMany()
                .HasForeignKey(x=> new {x.IdProduct, x.IdStore })
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
