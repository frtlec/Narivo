using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Narivo.Checkout.Consumer.Consumers;
using Narivo.Checkout.Core.Business.Services;
using Narivo.Checkout.Core.Clients.RefitClients;
using Narivo.Checkout.Core.Infastructure.Hubs;
using Narivo.Checkout.Core.Infastructure.Persistence;
using Narivo.Shared.Extensions;
using Narivo.Shared.Kafka;
using Refit;

var host = Host.CreateDefaultBuilder(args)
  .ConfigureAppConfiguration((context, config) =>
  {
      config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
      config.AddEnvironmentVariables();
  })
  .ConfigureServices((context, services) =>
  {
      services.AddTelemetry("Narivo.Checkout.Consumer");
      services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        context.Configuration.GetConnectionString("DefaultConnection"), sqlOptions => sqlOptions.MigrationsAssembly("Narivo.Checkout.API"))
);
      services.AddScoped<IOrderService, OrderService>();
      services.AddScoped<ICheckoutService, CheckoutService>();
      services.AddScoped<IProductService, ProductService>();
      services.AddScoped<KafkaProducer>();
      services.AddSignalRCore();
      // Kafka ayarlarını IOptions pattern ile ekle

      var t = context.Configuration.GetSection("KafkaConfig");
      services.Configure<KafkaConfig>(context.Configuration.GetSection("KafkaConfig"));



      // -----------------------------
      // Refit Client for Payment API
      // -----------------------------
      services.AddRefitClient<IMyPayNetApiClient>()
          .ConfigureHttpClient(c =>
          {
              c.BaseAddress = new Uri(context.Configuration.GetSection("MyNetApiConfig:BaseUrl").Value ?? string.Empty);
          });

      services.AddRefitClient<IMembershipApiClient>()
          .ConfigureHttpClient(c =>
          {
              c.BaseAddress = new Uri(context.Configuration.GetSection("MemberApiConfig:BaseUrl").Value ?? string.Empty);
          });

      services.AddRefitClient<ICatalogApiClient>()
          .ConfigureHttpClient(c =>
          {
              c.BaseAddress = new Uri(context.Configuration.GetSection("CatalogApiConfig:BaseUrl").Value ?? string.Empty);
          });

      // Kafka consumer servisi
      services.AddHostedService<CheckoutConsumer>();
      services.AddHostedService<OrderShippedConsumer>();

      //hub
  })
  .Build();

await host.RunAsync();