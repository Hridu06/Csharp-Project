using Blazor;
using Blazor.Layout;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http;
using VisitorManagementSystem.Blazor;
using VisitorManagementSystem.Blazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// ✅ Configure HttpClient with your backend API URL
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7005/") // <-- Your backend API base URL
});

// ✅ Register AuthService
builder.Services.AddScoped<AuthService>();

// ✅ Register MainLayout as a scoped service
builder.Services.AddScoped<MainLayout>();

// ✅ Build app and store ServiceProvider (so we can access MainLayout in static JS calls)
var host = builder.Build();
ProgramServiceProvider.ServiceProvider = host.Services; // 👈 store the DI container

await host.RunAsync();

// ✅ Define helper static class for DI access (same file, bottom)
public static class ProgramServiceProvider
{
    public static IServiceProvider? ServiceProvider { get; set; }
}
