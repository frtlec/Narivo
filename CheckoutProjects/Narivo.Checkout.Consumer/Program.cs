using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Narivo.Checkout.Core.Business.Services;
using Narivo.Checkout.Core.Clients.RefitClients;
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

      services.AddScoped<IOrderService, OrderService>();
      services.AddScoped<ICheckoutService, CheckoutService>();
      services.AddScoped<KafkaProducer>();
      // Kafka ayarlarını IOptions pattern ile ekle
      services.Configure<KafkaConfig>(context.Configuration.GetSection("KafkaConfig"));

      services.AddRefitClient<IMyPayNetApiClient>()
          .ConfigureHttpClient(c =>
          {
              c.BaseAddress = new Uri(context.Configuration.GetSection("MyNetApiConfig:BaseUrl").Value ?? string.Empty);
          });

      // Kafka consumer servisi
      services.AddHostedService<KafkaConsumerService>();
  })
  .Build();

await host.RunAsync();