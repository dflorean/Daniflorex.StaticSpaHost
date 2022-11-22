using Daniflorex.StaticSpaHost.Frontend7;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
//mod to lock prerendering
if (!builder.RootComponents.Any())
{
    builder.RootComponents.Add<App>("#app");
    builder.RootComponents.Add<HeadOutlet>("head::after");
}

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
//ConfigureServices(builder.Services, builder.HostEnvironment.BaseAddress);

var baseAddress = builder.Configuration.GetValue<string>("BaseUrl");
ConfigureServices(builder.Services, baseAddress);

await builder.Build().RunAsync();

static void ConfigureServices(IServiceCollection services, string baseAddress)
{
    services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseAddress) });
    //services.AddScoped<IFoo, MyFoo>();
}