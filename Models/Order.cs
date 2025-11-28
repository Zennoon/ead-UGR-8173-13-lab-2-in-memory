namespace PizzaStoreInMemory.Models
{
    public class Order
    {
        public int Id { get; set; }
        public List<PizzaOrder> PizzaOrders { get; set; } = [];
        public List<DrinkOrder> DrinkOrders { get; set; } = [];
    }
}
