using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http;
using VisitorManagementSystem.Application.Interfaces;
using VisitorManagementSystem.Blazor;
using VisitorManagementSystem.Blazor.Services;
using BlazorEmployeeService = VisitorManagementSystem.Blazor.Services.EmployeeService;
// 🔹 Alias to avoid interface ambiguity
using BlazorIEmployeeService = VisitorManagementSystem.Blazor.Services.IEmployeeService;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// 🔹 HttpClient pointing to your Web API
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7005/") });

// 🔹 Blazor services
builder.Services.AddScoped<BlazorIEmployeeService, BlazorEmployeeService>();
builder.Services.AddScoped<IVisitRequestService, VisitRequestService>();
builder.Services.AddScoped<IVisitorService, VisitorService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<VisitRequestService>();
builder.Services.AddScoped<NotificationService>();

builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
