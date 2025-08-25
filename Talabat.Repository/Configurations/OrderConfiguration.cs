using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Domain_Models.Order_Module;

namespace Talabat.Repository.Configurations
{
    class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // The properties of this reference type (Address) will be columns inside the Order table
            builder.OwnsOne(O => O.ShippingAddress);

            builder.Property(O => O.Status)
                .HasConversion(
                // Lambda expression for the conversion on insertion
                Status => Status.ToString(),
                // Lambda expression for conversion on fetching
                StatusString => (OrderStatus)Enum.Parse(typeof(OrderStatus), StatusString)
                );


            builder.HasOne(O => O.DeliveryMethod)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(O => O.Items)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
