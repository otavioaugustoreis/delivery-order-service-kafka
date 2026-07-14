namespace delivery_order_services.Application.Shared.Infra.Configuration
{
    public class MongoDbConfiguration
    {
        public string ConnectionString { get; set; } = default!;
        public string DatabaseName { get; set; } = default!;
    }
}
