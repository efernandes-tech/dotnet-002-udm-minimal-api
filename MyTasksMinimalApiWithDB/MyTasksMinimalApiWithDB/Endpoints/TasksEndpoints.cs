using Dapper.Contrib.Extensions;
using static MyTasksMinimalApiWithDB.Data.TaskContext;

namespace MyTasksMinimalApiWithDB.Endpoints;

public static class TasksEndpoints
{
    public static void MapTasksEndpoints(this WebApplication app)
    {
        app.MapGet("/", () => $"Welcome the MyTasksMinimalApi - {DateTime.Now}");

        app.MapGet("/tasks", async (GetConnection connectionGetter) =>
        {
            using var con = await connectionGetter();
            var tasks = con.GetAll<Data.Task>().ToList();
            if (tasks is null)
                return Results.NotFound();
            return Results.Ok(tasks);
        });

        app.MapGet("/tasks/{id:int}", async (GetConnection connectionGetter, int id) =>
        {
            using var con = await connectionGetter();
            return con.Get<Data.Task>(id) is Data.Task task
                ? Results.Ok(task)
                : Results.NotFound();
        });

        app.MapPost("/tasks", async (GetConnection connectionGetter, Data.Task task) =>
        {
            using var con = await connectionGetter();
            var id = con.Insert(task);
            return Results.Created($"/tasks/{id}", task);
        });

        app.MapPut("/tasks", async (GetConnection connectionGetter, Data.Task task) =>
        {
            using var con = await connectionGetter();
            var result = con.Update(task);
            return Results.Ok();
        });

        app.MapDelete("/tasks/{id:int}", async (GetConnection connectionGetter, int id) =>
        {
            using var con = await connectionGetter();
            var deleted = con.Get<Data.Task>(id);
            if (deleted is null)
                return Results.NotFound();
            con.Delete(deleted);
            return Results.Ok(deleted);
        });
    }
}
