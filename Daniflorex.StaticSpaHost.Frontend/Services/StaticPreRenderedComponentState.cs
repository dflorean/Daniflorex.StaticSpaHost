using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Daniflorex.StaticSpaHost.Frontend.Services;

public class StaticPreRenderedComponentState
{
    private readonly HttpClient http;

    private readonly NavigationManager nav;

    private readonly IWebAssemblyHostEnvironment hostEnv;

    private readonly PersistentComponentState componentState;

    public StaticPreRenderedComponentState(
        HttpClient http,
        NavigationManager nav,
        IWebAssemblyHostEnvironment hostEnv,
        PersistentComponentState componentState)
    {
        this.http = http;
        this.componentState = componentState;
        this.nav = nav;
        this.hostEnv = hostEnv;
    }

    public PersistingComponentStateSubscription RegisterOnPersisting(Func<Task> callback)
    {
        return this.componentState.RegisterOnPersisting(callback);
    }

    public async ValueTask<(bool found, TValue? instance)> TryTakeFromJsonAsync<TValue>(string key)
    {
        if (this.componentState.TryTakeFromJson(key, out TValue? instance)) return (true, instance);

        if (this.hostEnv.IsProduction())
        {
            // Fetch the HTML file statically rendered of the current page from a static web server.
            // NOTICE: If you pre - rendered with the "BlazorWasmPrerenderingOutputStyle=AppendHtmlExtension" option, you should fetch the URL `uri.AbsolutePath.TrimEnd('/') + ".html"`.
            var uri = new Uri(this.nav.Uri);
            var staticPrerenderedHTML = await this.http.GetStringAsync(uri.AbsolutePath.TrimEnd('/') + "/index.html");

            // Try to find the component state that is base64 encoded from inside the prerendered HTML text.
            var componentStateFragment = Regex.Match(staticPrerenderedHTML, @"Blazor-Component-State:(?<state>[a-zA-Z0-9+/=]+)");
            if (componentStateFragment.Success)
            {
                // If it is found, decode the base64 encoded state text and deserialize it since the format of the decoded text is JSON.
                var stateBytes = Convert.FromBase64String(componentStateFragment.Groups["state"].Value);
                var stateKeyValues = JsonSerializer.Deserialize<Dictionary<string, string>>(stateBytes);
                if (stateKeyValues != null)
                {
                    if (stateKeyValues.TryGetValue(key, out var base64Vlaue))
                    {
                        var valueBytes = Convert.FromBase64String(base64Vlaue);
                        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        var value = JsonSerializer.Deserialize<TValue?>(valueBytes, options);
                        return (true, value);
                    }
                }
            }
        }
        return (false, default(TValue));
    }

    public void PersistAsJson<TValue>(string key, TValue instance)
    {
        this.componentState.PersistAsJson(key, instance);
    }
}