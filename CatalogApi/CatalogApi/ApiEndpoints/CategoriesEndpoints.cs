using CatalogApi.Context;
using CatalogApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogApi.ApiEndpoints;

public static class CategoriesEndpoints
{
    public static void MapCategoriesEndpoints(this WebApplication app)
    {
        app.MapPost("/categories", async ([FromBody] Category category, [FromServices] AppDbContext db) =>
        {
            db.Categories.Add(category);
            await db.SaveChangesAsync();

            return Results.Created($"/categories/{category.Id}", category);
        })
            .Accepts<Category>("application/json")
            .Produces<Category>(StatusCodes.Status201Created)
            .WithName("CreatedNewCategory")
            .WithTags("Setter");

        app.MapGet("/categories", async (AppDbContext db) =>
            await db.Categories.ToListAsync()).WithTags("Categories")
                .RequireAuthorization();

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
    }
}
