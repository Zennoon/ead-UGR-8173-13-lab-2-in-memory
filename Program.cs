using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using PizzaStoreInMemory.Data;
using PizzaStoreInMemory.DTOs.Drink;
using PizzaStoreInMemory.DTOs.Pizza;
using PizzaStoreInMemory.DTOs.Topping;
using PizzaStoreInMemory.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PizzaStoreInMemory API",
        Description = "Making the pizzas you love!",
        Version = "v1"
    }
    );
});

builder.Services.AddDbContext<PizzaStoreInMemoryDbContext>(config =>
{
    config.UseInMemoryDatabase("pizzas");
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(config =>
    {
        config.SwaggerEndpoint("/swagger/v1/swagger.json", "PizzaStoreInMemory API v1");
    });
}

app.MapGet("/", () => "Hello World!");

app.MapGet("/pizzas", GetPizzas);
app.MapGet("/pizzas/{id}", GetPizza);
app.MapPost("/pizzas", CreatePizza);
app.MapPut("/pizzas/{id}", UpdatePizza);
app.MapDelete("/pizzas/{id}", DeletePizza);

app.MapGet("/toppings", GetToppings);
app.MapGet("/toppings/{id}", GetTopping);
app.MapGet("/pizzas/{pizzaId}/toppings", GetPizzaToppings);
app.MapPost("/toppings", CreateTopping);
app.MapPost("/pizzas/{pizzaId}/toppings/{toppingId}", AddToppingOptionToPizza);
app.MapPut("/toppings/{id}", UpdateTopping);
app.MapDelete("/toppings/{id}", DeleteTopping);
app.MapDelete("/pizzas/{pizzaId}/toppings/{toppingId}", RemoveToppingOptionFromPizza);

app.MapGet("/drinks", GetDrinks);
app.MapGet("/drinks/{id}", GetDrink);
app.MapPost("/drinks", CreateDrink);
app.MapPut("/drinks/{id}", UpdateDrink);
app.MapDelete("/drinks/{id}", DeleteDrink);

app.MapGet("/orders", GetOrders);
app.MapGet("/orders/{id}", GetOrder);
app.MapPost("/orders", CreateOrder);
app.MapPost("/orders/{orderId}/pizzas/{pizzaId}", AddPizzaToOrder);
app.MapPost("/orders/{orderId}/pizzas/{pizzaId}/toppings/{toppingId}", AddPizzaToppingToOrder);
app.MapPost("/orders/{orderId}/drinks/{drinkId}", AddDrinkToOrder);
app.MapPut("/orders/{id}", UpdateOrder);
app.MapDelete("/orders/{orderId}/pizzas/{pizzaId}", RemovePizzaFromOrder);
app.MapDelete("/orders/{orderId}/toppings/{toppingId}", RemoveToppingFromOrder);
app.MapDelete("/orders/{orderId}/drinks/{drinkId}", RemoveDrinkFromOrder);
app.MapDelete("/orders/{id}", DeleteOrder);

app.Run();

static async Task<IResult> GetPizzas(PizzaStoreInMemoryDbContext dbContext)
{
    return TypedResults.Ok(await dbContext.Pizzas.ToListAsync());
}

static async Task<IResult> GetPizza(int id, PizzaStoreInMemoryDbContext dbContext)
{
    var pizza = await dbContext.Pizzas.FindAsync(id);
    if (pizza is null) return TypedResults.NotFound();
    return TypedResults.Ok(pizza);
}

static async Task<IResult> CreatePizza(CreatePizzaDTO dto, PizzaStoreInMemoryDbContext dbContext)
{
    var pizza = new Pizza()
    {
        Name = dto.Name,
        Description = dto.Description,
        BasePrice = dto.BasePrice
    };

    await dbContext.Pizzas.AddAsync(pizza);
    await dbContext.SaveChangesAsync();
    return TypedResults.Created($"/pizzas/{pizza.Id}");
}

static async Task<IResult> UpdatePizza(int id, CreatePizzaDTO dto, PizzaStoreInMemoryDbContext dbContext)
{
    var pizza = await dbContext.Pizzas.FindAsync(id);

    if (pizza is null) return TypedResults.NotFound();
    pizza.Name = dto.Name;
    pizza.Description = dto.Description;
    pizza.BasePrice = dto.BasePrice;

    await dbContext.SaveChangesAsync();
    return TypedResults.NoContent();
}

static async Task<IResult> DeletePizza(int id, PizzaStoreInMemoryDbContext dbContext)
{
    var pizza = await dbContext.Pizzas.FindAsync(id);
    if (pizza is null) return TypedResults.NotFound();

    dbContext.Pizzas.Remove(pizza);
    await dbContext.SaveChangesAsync();
    return TypedResults.Ok();
}

static async Task<IResult> GetToppings(PizzaStoreInMemoryDbContext dbContext)
{
    return TypedResults.Ok(await dbContext.Toppings.ToListAsync());
}

static async Task<IResult> GetTopping(int id, PizzaStoreInMemoryDbContext dbContext)
{
    var topping = await dbContext.Toppings.FindAsync(id);

    if (topping is null) return TypedResults.NotFound();

    return TypedResults.Ok(topping);
}

static async Task<IResult> GetPizzaToppings(int pizzaId, PizzaStoreInMemoryDbContext dbContext)
{
    var pizza = await dbContext.Pizzas.FindAsync(pizzaId);

    if (pizza is null) return TypedResults.NotFound();

    return TypedResults.Ok(await dbContext.Toppings.Where(topping => topping.Pizzas.Contains(pizza)).ToListAsync());
}

static async Task<IResult> CreateTopping(CreateToppingDTO dto, PizzaStoreInMemoryDbContext dbContext)
{
    var topping = new Topping()
    {
        Name = dto.Name,
        Description = dto.Description,
        Price = dto.Price
    };

    await dbContext.Toppings.AddAsync(topping);
    await dbContext.SaveChangesAsync();
    return TypedResults.Created($"/toppings/{topping.Id}");
}

static async Task<IResult> AddToppingOptionToPizza(int pizzaId, int toppingId, PizzaStoreInMemoryDbContext dbContext)
{
    var pizza = await dbContext.Pizzas.FindAsync(pizzaId);
    var topping = await dbContext.Toppings.FindAsync(toppingId);

    if (pizza is null || topping is null) return TypedResults.NotFound();
    if (pizza.Toppings.Contains(topping)) return TypedResults.BadRequest("Pizza already has this topping option.");

    pizza.Toppings.Add(topping);
    await dbContext.SaveChangesAsync();
    return TypedResults.NoContent();
}

static async Task<IResult> UpdateTopping(int id, CreateToppingDTO dto, PizzaStoreInMemoryDbContext dbContext)
{
    var topping = await dbContext.Toppings.FindAsync(id);

    if (topping is null) return TypedResults.NotFound();

    topping.Name = dto.Name;
    topping.Description = dto.Description;
    topping.Price = dto.Price;

    await dbContext.SaveChangesAsync();
    return TypedResults.NoContent();
}

static async Task<IResult> DeleteTopping(int id, PizzaStoreInMemoryDbContext dbContext)
{
    var topping = await dbContext.Toppings.FindAsync(id);

    if (topping is null) return TypedResults.NotFound();

    dbContext.Toppings.Remove(topping);
    await dbContext.SaveChangesAsync();
    return TypedResults.NoContent();
}

static async Task<IResult> RemoveToppingOptionFromPizza(int pizzaId, int toppingId, PizzaStoreInMemoryDbContext dbContext)
{
    var pizza = await dbContext.Pizzas.FindAsync(pizzaId);
    var topping = await dbContext.Toppings.FindAsync(toppingId);

    if (pizza is null || topping is null) return TypedResults.NotFound();

    if (!pizza.Toppings.Contains(topping)) return TypedResults.BadRequest("Pizza doesn't have this topping option.");

    pizza.Toppings.Remove(topping);
    await dbContext.SaveChangesAsync();
    return TypedResults.Ok();
}

static async Task<IResult> GetDrinks(PizzaStoreInMemoryDbContext dbContext)
{
    return TypedResults.Ok(await dbContext.Drinks.ToListAsync());
}

static async Task<IResult> GetDrink(int id, PizzaStoreInMemoryDbContext dbContext)
{
    var drink = await dbContext.Drinks.FindAsync(id);
    if (drink is null) return TypedResults.NotFound();
    return TypedResults.Ok(drink);
}

static async Task<IResult> CreateDrink(CreateDrinkDTO dto, PizzaStoreInMemoryDbContext dbContext)
{
    var drink = new Drink()
    {
        Name = dto.Name,
        Description = dto.Description,
        Price = dto.Price
    };

    await dbContext.Drinks.AddAsync(drink);
    await dbContext.SaveChangesAsync();
    return TypedResults.Created($"/drinks/{drink.Id}");
}

static async Task<IResult> UpdateDrink(int id, CreateDrinkDTO dto, PizzaStoreInMemoryDbContext dbContext)
{
    var drink = await dbContext.Drinks.FindAsync(id);

    if (drink is null) return TypedResults.NotFound();
    drink.Name = dto.Name;
    drink.Description = dto.Description;
    drink.Price = dto.Price;

    await dbContext.SaveChangesAsync();
    return TypedResults.NoContent();
}

static async Task<IResult> DeleteDrink(int id, PizzaStoreInMemoryDbContext dbContext)
{
    var drink = await dbContext.Drinks.FindAsync(id);
    if (drink is null) return TypedResults.NotFound();

    dbContext.Drinks.Remove(drink);
    await dbContext.SaveChangesAsync();
    return TypedResults.Ok();
}

static async Task<IResult> GetOrders(bool? completed, PizzaStoreInMemoryDbContext dbContext)
{
    var query = dbContext.Orders.AsQueryable();

    if (completed is not null)
    {
        query = query.Where(order => order.IsCompleted == completed);
    }

    return TypedResults.Ok(await query.ToListAsync());
}

static async Task<IResult> GetOrder(int id, PizzaStoreInMemoryDbContext dbContext)
{
    var order = await dbContext.Orders.FindAsync(id);

    if (order is null) return TypedResults.NotFound();

    return TypedResults.Ok(order);
}

static async Task<IResult> CreateOrder(PizzaStoreInMemoryDbContext dbContext)
{
    var order = new Order();

    await dbContext.Orders.AddAsync(order);
    return TypedResults.Created($"/orders/{order.Id}");
}

static async Task<IResult> AddPizzaToOrder(int orderId, int pizzaId, int quantity, PizzaStoreInMemoryDbContext dbContext)
{
    var order = await dbContext.Orders.FindAsync(orderId);
    var pizza = await dbContext.Pizzas.FindAsync(pizzaId);

    if (order is null || pizza is null) return TypedResults.NotFound();

    if (quantity <= 0) return TypedResults.BadRequest("Please enter a valid amount.");

    var existingPizzaOrder = order.PizzaOrders.Where(pizzaOrder => pizzaOrder.PizzaId == pizza.Id).First();

    if (existingPizzaOrder is not null)
    {
        existingPizzaOrder.Quantity += quantity;
    }
    else
    {
        var pizzaOrder = new PizzaOrder()
        {
            OrderId = order.Id,
            PizzaId = pizza.Id,
            Quantity = quantity
        };

        await dbContext.PizzaOrders.AddAsync(pizzaOrder);
    }

    await dbContext.SaveChangesAsync();
    return TypedResults.NoContent();
}

static async Task<IResult> AddPizzaToppingToOrder(int orderId, int pizzaId, int toppingId, PizzaStoreInMemoryDbContext dbContext)
{
    var order = await dbContext.Orders.FindAsync(orderId);
    var pizza = await dbContext.Pizzas.FindAsync(pizzaId);
    var topping = await dbContext.Toppings.FindAsync(toppingId);

    if (order is null || pizza is null || topping is null)
    {
        return TypedResults.NotFound();
    }

    if (order.PizzaOrders.Where(pizzaOrder => pizzaOrder.PizzaId == pizza.Id) is null)
    {
        return TypedResults.BadRequest("Pizza hasn't been added to this order.");
    }

    if (pizza.Toppings.Where(t => t.Id == topping.Id) is null)
    {
        return TypedResults.BadRequest("Pizza doesn't have this topping option.");
    }

    order.Toppings.Add(topping);
    await dbContext.SaveChangesAsync();
    return TypedResults.NoContent();
}

static async Task<IResult> AddDrinkToOrder(int orderId, int drinkId, int quantity, PizzaStoreInMemoryDbContext dbContext)
{
    var order = await dbContext.Orders.FindAsync(orderId);
    var drink = await dbContext.Drinks.FindAsync(drinkId);

    if (order is null || drink is null) return TypedResults.NotFound();

    if (quantity <= 0) return TypedResults.BadRequest("Please enter a valid amount.");

    var existingDrinkOrder = order.DrinkOrders.Where(drinkOrder => drinkOrder.DrinkId == drink.Id).First();

    if (existingDrinkOrder is not null)
    {
        existingDrinkOrder.Quantity += quantity;
    }
    else
    {
        var drinkOrder = new DrinkOrder()
        {
            OrderId = order.Id,
            DrinkId = drink.Id,
            Quantity = quantity
        };

        await dbContext.DrinkOrders.AddAsync(drinkOrder);
    }

    await dbContext.SaveChangesAsync();
    return TypedResults.NoContent();
}

static async Task<IResult> UpdateOrder(int id, bool completed, PizzaStoreInMemoryDbContext dbContext)
{
    var order = await dbContext.Orders.FindAsync(id);

    if (order is null) return TypedResults.NotFound();

    order.IsCompleted = completed;
    await dbContext.SaveChangesAsync();
    return TypedResults.NoContent();
}

static async Task<IResult> RemovePizzaFromOrder(int orderId, int pizzaId, PizzaStoreInMemoryDbContext dbContext)
{
    var order = await dbContext.Orders.FindAsync(orderId);
    var pizza = await dbContext.Pizzas.FindAsync(pizzaId);

    if (order is null || pizza is null) return TypedResults.NotFound();

    var existingPizzaOrder = order.PizzaOrders.Where(pizzaOrder => pizzaOrder.PizzaId == pizza.Id).First();
    if (existingPizzaOrder is null)
    {
        return TypedResults.BadRequest("Pizza hasn't been added to this order.");
    }

    order.PizzaOrders.Remove(existingPizzaOrder);
    dbContext.PizzaOrders.Remove(existingPizzaOrder);
    await dbContext.SaveChangesAsync();
    return TypedResults.NoContent();
}

static async Task<IResult> RemoveToppingFromOrder(int orderId, int toppingId, PizzaStoreInMemoryDbContext dbContext)
{
    var order = await dbContext.Orders.FindAsync(orderId);
    var topping = await dbContext.Toppings.FindAsync(toppingId);

    if (order is null || topping is null) return TypedResults.NotFound();

    var existingTopping = order.Toppings.Where(t => t.Id == topping.Id).First();
    if (existingTopping is null)
    {
        return TypedResults.BadRequest("Topping hasn't been added to this order.");
    }

    order.Toppings.Remove(existingTopping);
    await dbContext.SaveChangesAsync();
    return TypedResults.NoContent();
}

static async Task<IResult> RemoveDrinkFromOrder(int orderId, int drinkId, PizzaStoreInMemoryDbContext dbContext)
{
    var order = await dbContext.Orders.FindAsync(orderId);
    var drink = await dbContext.Drinks.FindAsync(drinkId);

    if (order is null || drink is null) return TypedResults.NotFound();

    var existingDrinkOrder = order.DrinkOrders.Where(drinkOrder => drinkOrder.DrinkId == drink.Id).First();
    if (existingDrinkOrder is null)
    {
        return TypedResults.BadRequest("Drink hasn't been added to this order.");
    }

    order.DrinkOrders.Remove(existingDrinkOrder);
    dbContext.DrinkOrders.Remove(existingDrinkOrder);
    await dbContext.SaveChangesAsync();
    return TypedResults.NoContent();
}

static async Task<IResult> DeleteOrder(int id, PizzaStoreInMemoryDbContext dbContext)
{
    var order = await dbContext.Orders.FindAsync(id);

    if (order is null) return TypedResults.NotFound();

    dbContext.Orders.Remove(order);
    await dbContext.SaveChangesAsync();
    return TypedResults.NoContent();
}
