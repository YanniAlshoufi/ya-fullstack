using Src.Shared.ClassLib.ServicesInterfaces;
using Src.Web.Services;

namespace Src.Web.Helpers;

public static class ServiceGeneration
{
    public static void AddServicesAutomatically(WebApplicationBuilder builder)
    {
        // Auto-added services, DO NOT REMOVE THIS LINE
        builder.Services.AddScoped<IUsersService, UsersService>();
    }
}