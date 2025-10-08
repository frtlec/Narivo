using Microsoft.EntityFrameworkCore;
using Narivo.Shared.Kafka;
using Narivo.Shared.Middlewares;
using Narivo.Shipping.Core;
using Narivo.Shipping.Core.Integration;
using Refit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<ShipmentProcessService>();
builder.Services.AddScoped<KafkaProducer>();
builder.Services.Configure<KafkaConfig>(builder.Configuration.GetSection("KafkaConfig"));
builder.Services.AddDbContext<ShippingProcessDbContext>(options =>
 options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"), sqlOptions => sqlOptions.MigrationsAssembly("Narivo.ShippingApi"))
);
// Refit client ekleme
builder.Services.AddRefitClient<IFakeShippingCompanyApiRefitClient>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri(builder.Configuration.GetSection("FakeShippingCompanyApi:BaseUrl").Value ?? string.Empty);
    });

builder.Services.AddRefitClient<ICheckoutApiRefitClient>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri(builder.Configuration.GetSection("CheckoutApiConfig:BaseUrl").Value ?? string.Empty);
    });

builder.Services.AddRefitClient<IMembershipApiRefitClient>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri(builder.Configuration.GetSection("MemberApiConfig:BaseUrl").Value ?? string.Empty);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
