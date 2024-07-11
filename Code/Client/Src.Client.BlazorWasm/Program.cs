using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Src.Client.BlazorWasm;
using Src.Client.BlazorWasm.Helpers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
{
    #region Boilerplate
    ServiceGeneration.AddServicesAutomatically(builder);
    builder.RootComponents.Add<App>("#app");
    builder.RootComponents.Add<HeadOutlet>("head::after");

    builder.Services.AddScoped(_ =>
        new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
    #endregion
    
    await builder.Build().RunAsync();
}