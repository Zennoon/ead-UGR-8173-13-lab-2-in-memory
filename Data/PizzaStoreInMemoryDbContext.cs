using Microsoft.EntityFrameworkCore;
using PizzaStoreInMemory.Models;

namespace PizzaStoreInMemory.Data
{
    public class PizzaStoreInMemoryDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Topping> Toppings { get; set; }
        public DbSet<Drink> Drinks { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<PizzaOrder> PizzaOrders { get; set; }
        public DbSet<DrinkOrder> DrinkOrders { get; set; }
    }
}
