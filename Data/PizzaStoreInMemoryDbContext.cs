using Microsoft.EntityFrameworkCore;
using PizzaStoreInMemory.Models;

namespace PizzaStoreInMemory.Data
{
    public class PizzaStoreInMemoryDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Pizza> Pizzas { get; set; }
    }
}
