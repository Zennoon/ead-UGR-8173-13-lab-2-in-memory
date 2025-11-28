namespace PizzaStoreInMemory.Models
{
    public class Topping
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public float Price { get; set; }
        // Skip navigation
        public List<Pizza> Pizzas { get; set; } = [];
    }
}
