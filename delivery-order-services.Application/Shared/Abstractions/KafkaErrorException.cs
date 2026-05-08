namespace delivery_order_services.Application.Shared.Abstractions
{
    public class KafkaErrorException : Exception
    {
        private const string MESSAGE_COMMON = "An error occurred in Kafka message production. Input:{@input}";
        public KafkaErrorException(string message = MESSAGE_COMMON) : base(message)
        {
        }
    }
}
