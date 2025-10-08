using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Narivo.Orchestrator.Consumers;
using Narivo.Orchestrator.EventStore;
using Narivo.Orchestrator.Sync;
using Narivo.Shared.Extensions;
using Narivo.Shared.Kafka;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
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
        services.Configure<EventStoreSettings>(context.Configuration.GetSection("EventStore"));
        services.Configure<KafkaConfig>(context.Configuration.GetSection("KafkaConfig"));
        // CheckoutConsumer'ı DI container'a ekliyoruz
        services.AddScoped<EventStoreRepository>();
        services.AddScoped<KafkaProducer>();
        services.AddHostedService<CheckoutInitialConsumer>();
        services.AddHostedService<CheckoutSuccessfulConsumer>();
        services.AddHostedService<CheckoutFailConsumer>();
        //services.AddHostedService<OrderShippedConsumer>();
        services.AddTelemetry("Narivo.Orchestrator");

        //refit client ekle
        services.AddRefitClient<ICheckoutApiClient>()
                    .ConfigureHttpClient(c =>
                    {
                        c.BaseAddress = new Uri(context.Configuration.GetSection("CheckoutApiConfig:BaseUrl").Value ?? string.Empty);
                    });
                    
    })
    .Build();

await host.RunAsync();