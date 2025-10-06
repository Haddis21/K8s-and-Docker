using Blazored.LocalStorage;
using FoodOrderApp.Components;
using FoodOrderApp.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Win32;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();



// 
builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiUrls:UserService"] ?? "http://localhost:8080");
});

builder.Services.AddHttpClient<IUserService, UserService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiUrls:UserService"] ?? "http://localhost:8080");
});

builder.Services.AddHttpClient<IRestaurantService, RestaurantService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiUrls:RestaurantService"] ?? "http://localhost:8081");
});

builder.Services.AddHttpClient<IOrderService, OrderService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiUrls:OrderService"] ?? "http://localhost:8082");
});

// Register services
//builder.Services.AddScoped<IAuthService, AuthService>();
//builder.Services.AddScoped<IUserService, UserService>();
//builder.Services.AddScoped<IRestaurantService, RestaurantService>();
//builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<CartService>();

// Authentication
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();


//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7275") });
// builder.Services.AddScoped(sp => new HttpClient());

// builder.Services.AddHttpClient("Api", client =>
// {
//     client.BaseAddress = new Uri("http://localhost:5078/"); // gateway or k8s ingress later
// });



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
