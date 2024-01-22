using Ecommerce.Models.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.DAL.Configuration
{
    public class CompanyConfiguration : IEntityTypeConfiguration<CompanyModel>
    {
        public void Configure(EntityTypeBuilder<CompanyModel> builder)
        {
            builder.HasKey(c => c.IdCompany);
            builder.Property(c => c.Name).IsRequired().HasMaxLength(60);
            builder.Property(c => c.Description).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Country).IsRequired().HasMaxLength(50);
            builder.Property(c => c.City).IsRequired().HasMaxLength(150);
            builder.Property(c => c.Address).IsRequired().HasMaxLength(250);
            builder.Property(c => c.PhoneNumber).IsRequired().HasMaxLength(50);

            //llaves foraneas
            builder.Property(c => c.IdStore).IsRequired();
            builder.Property(c => c.CreateUserId).IsRequired(false);
            builder.Property(c => c.UpdateUserId).IsRequired(false);

            //Relaciones

            builder.HasOne(c=>c.Store).WithMany()
                .HasForeignKey(c=>c.IdStore)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(c=>c.CreateUser).WithMany()
                .HasForeignKey(c=>c.CreateUserId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(c=>c.UpdateUser).WithMany()
                .HasForeignKey(c=>c.UpdateUserId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
