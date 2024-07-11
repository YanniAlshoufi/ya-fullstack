using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Src.Client.BlazorWasm.Services;
using Src.Shared.ClassLib.ServicesInterfaces;

namespace Src.Client.BlazorWasm.Helpers;

public static class ServiceGeneration
{
    public static void AddServicesAutomatically(WebAssemblyHostBuilder builder)
    {
        // Auto-added services, DO NOT REMOVE THIS LINE
        builder.Services.AddScoped<IUsersService, UsersService>();
    }
}