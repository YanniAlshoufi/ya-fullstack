using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Client;
using Client.Services;
using Shared.ServicesInterfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
{
    #region Custom Services
    builder.Services.AddScoped<IUsersService, UsersService>();
    #endregion
    
    #region Boilerplate
    builder.RootComponents.Add<App>("#app");
    builder.RootComponents.Add<HeadOutlet>("head::after");

    builder.Services.AddScoped(_ =>
        new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
    #endregion
    
    await builder.Build().RunAsync();
}

