namespace PizzaStoreInMemory.Models
{
    public class Pizza
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public float BasePrice { get; set; }
        // Skip navigation
        public List<Topping> Toppings { get; set; } = [];
        public List<PizzaOrder> PizzaOrders { get; set; } = [];
    }
}
