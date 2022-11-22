using Daniflorex.StaticSpaHost.Frontend;
using Daniflorex.StaticSpaHost.Frontend.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
//mod to lock prerendering
if (!builder.RootComponents.Any())
{
    builder.RootComponents.Add<App>("#app");
    builder.RootComponents.Add<HeadOutlet>("head::after");
}

//builder.RootComponents.Add<App>("#app");
//builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

if (builder.HostEnvironment.IsDevelopment())
{
    ConfigureServices(builder.Services, "https://localhost:44384");
}

if (builder.HostEnvironment.IsProduction())
{
    ConfigureServices(builder.Services, "https://staticspa-demo.azurewebsites.net");
}

var baseAddress = builder.Configuration.GetValue<string>("BaseUrl");
ConfigureServices(builder.Services, builder.HostEnvironment.BaseAddress);

await builder.Build().RunAsync();


static void ConfigureServices(IServiceCollection services, string baseAddress)
{
    services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseAddress) });
    services.AddScoped<StaticPreRenderedComponentState>();
}