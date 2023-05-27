using BlazorMix;
using BlazorMix.Docs;
using BlazorMix.Docs.Core.Services;
using BlazorMix.Docs.Internal;
using BlazorMix.Docs.Internal.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.RootComponents.RegisterCustomElements();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var userAgent =
          "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.129 Safari/537.36 Edg/81.0.416.68";

var baseUri = builder.HostEnvironment.BaseAddress;
builder.Services.AddScoped<DocService>();
builder.Services.AddScoped< IPrismHighlighter, PrismHighlighter> ();
builder.Services.AddHttpClient("docsHttp", httpClient =>
    {
        httpClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
        httpClient.BaseAddress = new Uri(baseUri);
    });
builder.Services
    .AddBlazorMix();
await builder.Build().RunAsync();
