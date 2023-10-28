using CatalogApi.Models;
using CatalogApi.Services;
using Microsoft.AspNetCore.Authorization;

namespace CatalogApi.ApiEndpoints;

public static class AuthenticationEndpoints
{
    public static void MapAuthenticationEndpoints(this WebApplication app)
    {
        app.MapPost("/login", [AllowAnonymous] (UserModel userModel, ITokenService tokenService) =>
        {
            if (userModel == null)
            {
                return Results.BadRequest("Invalid login");
            }
            if (userModel.UserName == "efernandes.tech" && userModel.Password == "12345")
            {
                var tokenString = tokenService.GerarToken(
                    app.Configuration["Jwt:Key"],
                    app.Configuration["Jwt:Issuer"],
                    app.Configuration["Jwt:Audience"],
                    userModel
                );
                return Results.Ok(new { token = tokenString });
            }
            else
            {
                return Results.BadRequest("Invalid login");
            }
        })
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status200OK)
            .WithName("Login")
            .WithTags("Authentication");
    }
}
