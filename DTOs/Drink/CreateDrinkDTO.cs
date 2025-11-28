namespace PizzaStoreInMemory.DTOs.Drink
{
    public class CreateDrinkDTO
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public float Price { get; set; }
    }
}
