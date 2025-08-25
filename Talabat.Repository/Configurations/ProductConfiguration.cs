using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Domain_Models;

namespace Talabat.Repository.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(P => P.brand).WithMany();
            builder.HasOne(P => P.category).WithMany();
            builder.Property(P => P.Name).IsRequired();
            builder.Property(P => P.PictureURL).IsRequired();
            builder.Property(P => P.Price).IsRequired();
            builder.Property(P => P.BrandID).IsRequired();
            builder.Property(P => P.CategoryID).IsRequired();
        }
    }
}
