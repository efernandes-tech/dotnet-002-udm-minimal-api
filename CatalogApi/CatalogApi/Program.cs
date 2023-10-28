using CatalogApi.Context;
using CatalogApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

app.MapGet("/", () => "Catalog API - 2023").ExcludeFromDescription();

app.MapPost("/categories", async ([FromBody] Category category, [FromServices] AppDbContext db) =>
{
    db.Categories.Add(category);
    await db.SaveChangesAsync();

    return Results.Created($"/categories/{category.Id}", category);
}).Accepts<Category>("application/json")
.Produces<Category>(StatusCodes.Status201Created)
.WithName("CreatedNewCategory")
.WithTags("Setter");

app.MapGet("/categories", async (AppDbContext db) => await db.Categories.ToListAsync());

app.MapGet("/categories/{id:int}", async (int id, AppDbContext db) =>
{
    return await db.Categories.FindAsync(id) is Category category
        ? Results.Ok(category)
        : Results.NotFound();
});

app.MapPut("/categories/{id:int}", async (int id, Category category, AppDbContext db) =>
{
    if (category.Id != id)
    {
        return Results.BadRequest();
    }

    var categoryDB = await db.Categories.FindAsync(id);

    if (categoryDB is null) return Results.NotFound();

    categoryDB.Name = category.Name;
    categoryDB.Description = category.Description;

    await db.SaveChangesAsync();

    return Results.Ok(categoryDB);
});

app.MapDelete("/categories/{id:int}", async (int id, AppDbContext db) =>
{
    var category = await db.Categories.FindAsync(id);

    if (category is null)
    {
        return Results.NotFound();
    }

    db.Categories.Remove(category);
    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapPost("/products", async (Product product, AppDbContext db) =>
{
    db.Products.Add(product);
    await db.SaveChangesAsync();

    return Results.Created($"/products/{product.Id}", product);
});

app.MapGet("/products", async (AppDbContext db) => await db.Products.ToListAsync());

app.MapGet("/products/{id:int}", async (int id, AppDbContext db) =>
{
    return await db.Products.FindAsync(id) is Product product
        ? Results.Ok(product)
        : Results.NotFound();
});

app.MapPut("/products/{id:int}", async (int id, Product product, AppDbContext db) =>
{
    if (product.Id != id)
    {
        return Results.BadRequest();
    }

    var productDB = await db.Products.FindAsync(id);

    if (productDB is null) return Results.NotFound();

    productDB.Name = product.Name;
    productDB.Description = product.Description;
    productDB.Price = product.Price;
    productDB.Image = product.Image;
    productDB.BuyDate = product.BuyDate;
    productDB.Quantity = product.Quantity;
    productDB.CategoryId = product.CategoryId;

    await db.SaveChangesAsync();

    return Results.Ok(productDB);
});

app.MapDelete("/products/{id:int}", async (int id, AppDbContext db) =>
{
    var product = await db.Products.FindAsync(id);

    if (product is null)
    {
        return Results.NotFound();
    }

    db.Products.Remove(product);
    await db.SaveChangesAsync();

    return Results.NoContent();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
