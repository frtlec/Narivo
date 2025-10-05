using Microsoft.EntityFrameworkCore;
using Narivo.Checkout.Core.Business.Services;
using Narivo.Checkout.Core.Clients.RefitClients;
using Narivo.Checkout.Core.Infastructure.Persistence;
using Narivo.Shared.Kafka;
using Narivo.Shared.Middlewares;
using Refit;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICheckoutService, CheckoutService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<KafkaProducer>();


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
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
