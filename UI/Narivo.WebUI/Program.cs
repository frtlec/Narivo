using Narivo.Checkout.Core.Business.Services;
using Narivo.Checkout.Core.Clients.RefitClients;
using Narivo.WebUI.Components;
using Narivo.WebUI.HttpClients;
using Narivo.WebUI.Services;
using Narivo.WebUI.StateServices;
using Refit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<IMemberService,MemberService>();
builder.Services.AddScoped<IProductService,ProductService>();

builder.Services.AddScoped<BasketService>();
builder.Services.AddScoped<OrderUIService>();
builder.Services.AddScoped<MemberUIService>();
builder.Services.AddScoped<CheckoutUIService>();
builder.Services.AddSingleton<AppState>();
builder.Services.AddRefitClient<IMembershipApiClient>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri(builder.Configuration.GetSection("MemberApiConfig:BaseUrl").Value ?? string.Empty);
    });

builder.Services.AddRefitClient<ICatalogApiClient>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri(builder.Configuration.GetSection("CatalogApiConfig:BaseUrl").Value ?? string.Empty);
    });

builder.Services.AddRefitClient<ICheckoutApiClient>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri(builder.Configuration.GetSection("CheckoutApiConfig:BaseUrl").Value ?? string.Empty);
    });

builder.Services.AddRefitClient<IShippingApiClient>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri(builder.Configuration.GetSection("ShippingApiConfig:BaseUrl").Value ?? string.Empty);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
