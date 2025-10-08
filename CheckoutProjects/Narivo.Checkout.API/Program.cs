using Microsoft.EntityFrameworkCore;
using Narivo.Checkout.Core.Business.Services;
using Narivo.Checkout.Core.Clients.RefitClients;
using Narivo.Checkout.Core.Infastructure.Hubs;
using Narivo.Checkout.Core.Infastructure.Persistence;
using Narivo.Shared.Kafka;
using Narivo.Shared.Middlewares;
using Refit;
using Scalar.AspNetCore;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using Narivo.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICheckoutService, CheckoutService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<KafkaProducer>();
builder.Services.AddSignalR();




builder.Services.Configure<MyNetApiConfig>(
    builder.Configuration.GetSection("MyNetApiConfig")
);
builder.Services.Configure<KafkaConfig>(
    builder.Configuration.GetSection("KafkaConfig")
);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"), sqlOptions => sqlOptions.MigrationsAssembly("Narivo.Checkout.API"))
);
// -----------------------------
// Refit Client for Payment API
// -----------------------------
builder.Services.AddRefitClient<IMyPayNetApiClient>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri(builder.Configuration.GetSection("MyNetApiConfig:BaseUrl").Value ?? string.Empty);
    });
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:5001") // Blazor portu
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

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

builder.Services.AddTelemetry("Narivo.Checkout.API");
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseCors();
app.UseAuthorization();

app.MapControllers();
app.MapHub<CheckoutHub>("/checkoutHub"); // <-- Hub mapping
app.MapHub<SimpleHub>("/simpleHub");
app.Run();