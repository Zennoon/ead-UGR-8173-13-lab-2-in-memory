using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PizzaStoreInMemory.Models;

namespace PizzaStoreInMemory.Data.Configurations
{
    public class DrinkOrderConfiguration : IEntityTypeConfiguration<DrinkOrder>
    {
        public void Configure(EntityTypeBuilder<DrinkOrder> builder)
        {
            builder.HasKey(drinkOrder => new { drinkOrder.DrinkId, drinkOrder.OrderId });
        }
    }
}
