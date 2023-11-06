using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

CreateHostBuilder().Build().Run();

static IHostBuilder CreateHostBuilder() =>Host.CreateDefaultBuilder()
    .ConfigureServices((hostContext, services) =>
{
   services.AddHostedService<RabbitMQConsumer>();
});