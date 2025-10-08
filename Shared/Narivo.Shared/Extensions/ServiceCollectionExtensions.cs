using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narivo.Shared.Extensions;

public static class ServiceCollectionExtensions
{

    public static void AddTelemetry(this IServiceCollection services, string projectName)
    {
        services.AddOpenTelemetry()
            .ConfigureResource(resource =>
                resource.AddService(projectName)
            )
            .WithTracing(tracer =>
            {
                tracer
                    .AddAspNetCoreInstrumentation() // API isteklerini takip eder
                    .AddHttpClientInstrumentation() // Diğer mikroservislere yapılan HTTP çağrılarını izler
                    .AddSource("Kafka")              // Kafka veya custom event source’ları eklemek için
                    .AddJaegerExporter(options =>
                    {
                        options.AgentHost = "localhost"; // Docker içindeyse "jaeger"
                        options.AgentPort = 6831;
                    });
            });
    }
}
