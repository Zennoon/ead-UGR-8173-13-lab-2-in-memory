namespace PizzaStoreInMemory.Models
{
    public class Order
    {
        public int Id { get; set; }
        public bool IsCompleted { get; set; }
        public List<PizzaOrder> PizzaOrders { get; set; } = [];
        public List<DrinkOrder> DrinkOrders { get; set; } = [];
        public List<Topping> Toppings { get; set; } = [];
    }
}
