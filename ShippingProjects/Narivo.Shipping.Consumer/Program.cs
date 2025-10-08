using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Narivo.Shared.Extensions;
using Narivo.Shared.Kafka;
using Narivo.Shipping.Core;
using Narivo.Shipping.Core.Integration;
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
      services.AddDbContext<ShippingProcessDbContext>(options =>
            options.UseSqlServer(
                context.Configuration.GetConnectionString("DefaultConnection"), sqlOptions => sqlOptions.MigrationsAssembly("Narivo.Checkout.API"))
        );

      services.AddScoped<KafkaProducer>();
      // Kafka ayarlarını IOptions pattern ile ekle

      var t = context.Configuration.GetSection("KafkaConfig");
      services.Configure<KafkaConfig>(context.Configuration.GetSection("KafkaConfig"));



      // -----------------------------
      // Refit Client for Payment API
      // -----------------------------
      services.AddRefitClient<IFakeShippingCompanyApiRefitClient>()
          .ConfigureHttpClient(c =>
          {
              c.BaseAddress = new Uri(context.Configuration.GetSection("FakeShippingCompanyApi:BaseUrl").Value ?? string.Empty);
          });

      //hub
  })
  .Build();

await host.RunAsync();