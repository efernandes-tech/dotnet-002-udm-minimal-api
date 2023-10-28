using CatalogApi.Context;
using CatalogApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogApi.ApiEndpoints;

public static class ProductsEndpoints
{
    public static void MapProductsEndpoints(this WebApplication app)
    {
        app.MapPost("/products", async (Product product, AppDbContext db) =>
        {
            db.Products.Add(product);
            await db.SaveChangesAsync();

            return Results.Created($"/products/{product.Id}", product);
        });

        app.MapGet("/products", async (AppDbContext db) =>
            await db.Products.ToListAsync()).WithTags("Products")
                .RequireAuthorization();

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
    }
}
