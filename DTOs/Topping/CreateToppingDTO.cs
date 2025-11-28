namespace PizzaStoreInMemory.DTOs.Topping
{
    public class CreateToppingDTO
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public float Price { get; set; }
    }
}
