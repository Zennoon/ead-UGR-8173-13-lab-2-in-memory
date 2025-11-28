namespace PizzaStoreInMemory.DTOs.Pizza
{
    public class CreatePizzaDTO
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public float BasePrice { get; set; }
    }
}
