using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
          .ConfigureHostConfiguration(configHost => {
              configHost.AddJsonFile("appsettings.json");
          })
          .ConfigureServices((hostContext, services) => {
              services.AddHostedService<RabbitMqConsumerAdd>();
              services.AddHostedService<RabbitMqConsumerDelete>();
          })

         .UseConsoleLifetime()
         .Build();

//run the host
host.Run();