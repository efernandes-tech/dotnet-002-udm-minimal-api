using MyTasksMinimalApiWithDB.Endpoints;
using MyTasksMinimalApiWithDB.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddPersistence();

var app = builder.Build();

app.MapTasksEndpoints();

app.Run();
