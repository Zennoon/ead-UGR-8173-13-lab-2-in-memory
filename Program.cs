using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using PizzaStoreInMemory.Data;

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

app.Run();
