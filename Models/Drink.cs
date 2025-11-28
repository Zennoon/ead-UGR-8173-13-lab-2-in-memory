namespace PizzaStoreInMemory.Models
{
    public class Drink
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public float Price { get; set; }
    }
}
