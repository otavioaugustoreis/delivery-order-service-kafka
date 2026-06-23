namespace delivery_order_services.Application.Shared.Contants
{
    public static class Topics
    {
        public const string OrderTopic = "order.topic-requested";
    }

    public static class ConsumerGroups
    {
        public const string OrderGroupId = "delivery.order-notifier";
    }
}
