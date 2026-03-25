using Zoo.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// API Endpoints
var animals = new List<Animal>();

app.MapGet("/api/animals", () => animals);

app.MapGet("/api/animals/{id:guid}", (Guid id) =>
{
    var animal = animals.FirstOrDefault(a => a.Id == id);
    return animal is null ? Results.NotFound() : Results.Ok(animal);
});

app.MapPost("/api/animals", (Animal animal) =>
{
    animals.Add(animal);
    return Results.Created($"/api/animals/{animal.Id}", animal);
});

app.MapDelete("/api/animals/{id:guid}", (Guid id) =>
{
    var animal = animals.FirstOrDefault(a => a.Id == id);
    if (animal is null) return Results.NotFound();
    animals.Remove(animal);
    return Results.NoContent();
});

app.Run();
