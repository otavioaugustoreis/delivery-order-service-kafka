namespace delivery_order_services.Application.Shared.Abstractions
{
    public interface TEnvelope<T>
    {
        string Key { get; set; }
        T Value { get; set; }
        string Topic { get; }
    }
}
