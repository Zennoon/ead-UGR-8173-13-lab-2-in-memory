using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using PizzaStoreInMemory.Models;

namespace PizzaStoreInMemory.Data.Configurations
{
    public class BoolValueGenerator : ValueGenerator<bool>
    {
        public override bool Next(EntityEntry entry)
        {
            return false;
        }

        public override bool GeneratesTemporaryValues => false;
    }
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(order => order.IsCompleted).HasValueGenerator<BoolValueGenerator>();
        }
    }
}
