using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PizzaStoreInMemory.Models;

namespace PizzaStoreInMemory.Data.Configurations
{
    public class PizzaOrderConfiguration : IEntityTypeConfiguration<PizzaOrder>
    {
        public void Configure(EntityTypeBuilder<PizzaOrder> builder)
        {
            builder.HasKey(pizzaOrder => new { pizzaOrder.PizzaId, pizzaOrder.OrderId });
        }
    }
}
