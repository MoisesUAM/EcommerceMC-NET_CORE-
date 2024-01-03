using Ecommerce.Models.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DAL.Configuration
{
    public class StoreConfiguration : IEntityTypeConfiguration<StoreModel>
    {
        public void Configure(EntityTypeBuilder<StoreModel> builder)
        {
            builder.Property(x=> x.IdStore).IsRequired();
            builder.Property(x=> x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x=> x.Description).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Estate).IsRequired();
        }
    }
}
