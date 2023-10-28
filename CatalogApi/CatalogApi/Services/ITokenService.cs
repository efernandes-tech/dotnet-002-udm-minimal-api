using CatalogApi.Models;

namespace CatalogApi.Services;

public interface ITokenService
{
    string GerarToken(string key, string issuer, string audience, UserModel user);
}
