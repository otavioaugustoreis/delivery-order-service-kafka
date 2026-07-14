using delivery_order_services.Notify.Consumer;
using delivery_order_services.Notify.ServicesCollectionExtensions;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddWorker(hostContext.Configuration);
                services.AddHostedService<OrderCreatedConsumer>();
            });
}