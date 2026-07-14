using delivery_order_services.Application.Shared.Abstractions.Result;
using delivery_order_services.Application.Shared.Infra.Repositories.Order;
using delivery_order_services.Controllers.Order.Models;
using delivery_order_services.Controllers.Order.UseCase;
using delivery_order_services.Producer;
using Microsoft.Extensions.Logging;
using DomainOrder = delivery_order_services.Application.Domain.Order;

namespace UnitTest.UseCase
{
    public class OrderEventUseCaseTests
    {
        [Fact]
        public async Task InsertOneAsync_WhenOrderIsNew_SetsCreatedAndPublishesMessage()
        {
            // Arrange
            var request = new OrderRequestModel("Notebook", "client-1");
            var repository = new FakeOrderRepository(insertResult: true);
            var producer = new FakeOrderProducer();
            var useCase = new OrderEventUseCase(repository, new NullLogger<OrderEventUseCase>(), producer);

            // Act
            var result = await useCase.InsertOneAsync(request, "idem-1", CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(repository.InsertedOrder);
            Assert.Equal("Notebook", repository.InsertedOrder!.ProductName);
            Assert.Equal("client-1", repository.InsertedOrder.ClientId);
            Assert.Equal("CREATED", repository.InsertedOrder.OrderStatus);
            Assert.Equal("idem-1", repository.InsertedOrder.IdempotencyKey);
            Assert.Single(producer.Envelopes);
            Assert.Equal("Notebook", producer.Envelopes[0].Value.ProductName);
            Assert.Equal("client-1", producer.Envelopes[0].Value.ClientId);
            Assert.Equal("CREATED", producer.Envelopes[0].Value.OrderStatus);
        }

        [Fact]
        public async Task InsertOneAsync_WhenOrderAlreadyExists_ReturnsConflictAndDoesNotPublish()
        {
            // Arrange
            var request = new OrderRequestModel("Notebook", "client-1");
            var repository = new FakeOrderRepository(insertResult: false);
            var producer = new FakeOrderProducer();
            var useCase = new OrderEventUseCase(repository, new NullLogger<OrderEventUseCase>(), producer);

            // Act
            var result = await useCase.InsertOneAsync(request, "idem-1", CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.Conflict, result.Error?.Code);
            Assert.Empty(producer.Envelopes);
        }

        [Fact]
        public async Task InsertOneAsync_WhenRepositoryThrows_ReturnsUnexpectedError()
        {
            // Arrange
            var request = new OrderRequestModel("Notebook", "client-1");
            var repository = new FakeOrderRepository(throwOnInsert: true);
            var producer = new FakeOrderProducer();
            var useCase = new OrderEventUseCase(repository, new NullLogger<OrderEventUseCase>(), producer);

            // Act
            var result = await useCase.InsertOneAsync(request, "idem-1", CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.UnexpectedError, result.Error?.Code);
            Assert.Empty(producer.Envelopes);
        }

        [Fact]
        public async Task FindByClientIdAsync_WhenRepositoryReturnsOrders_ReturnsSuccessWithOrders()
        {
            // Arrange
            var expectedOrders = new List<DomainOrder>
            {
                new() { ProductName = "Notebook", ClientId = "client-1", OrderStatus = "CREATED", IdempotencyKey = "idem-1" }
            };
            var repository = new FakeOrderRepository(findResult: expectedOrders);
            var producer = new FakeOrderProducer();
            var useCase = new OrderEventUseCase(repository, new NullLogger<OrderEventUseCase>(), producer);

            // Act
            var result = await useCase.FindByClientIdAsync("client-1", CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            var orders = Assert.IsType<List<DomainOrder>>(result.GetContent());
            Assert.Single(orders);
            Assert.Equal("Notebook", orders[0].ProductName);
            Assert.Equal("client-1", orders[0].ClientId);
        }

        [Fact]
        public async Task FindByClientIdAsync_WhenRepositoryThrows_ReturnsUnexpectedError()
        {
            // Arrange
            var repository = new FakeOrderRepository(throwOnFind: true);
            var producer = new FakeOrderProducer();
            var useCase = new OrderEventUseCase(repository, new NullLogger<OrderEventUseCase>(), producer);

            // Act
            var result = await useCase.FindByClientIdAsync("client-1", CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCode.UnexpectedError, result.Error?.Code);
        }

        private sealed class FakeOrderRepository : IOrderRepository
        {
            private readonly bool _insertResult;
            private readonly bool _throwOnInsert;
            private readonly bool _throwOnFind;
            private readonly List<DomainOrder> _findResult;

            public FakeOrderRepository(bool insertResult = true, bool throwOnInsert = false, bool throwOnFind = false, List<DomainOrder>? findResult = null)
            {
                _insertResult = insertResult;
                _throwOnInsert = throwOnInsert;
                _throwOnFind = throwOnFind;
                _findResult = findResult ?? new List<DomainOrder>();
            }

            public DomainOrder? InsertedOrder { get; private set; }

            public Task<List<DomainOrder?>?> FindByClientIdAsync(string id, CancellationToken cancellationToken)
            {
                if (_throwOnFind)
                {
                    throw new InvalidOperationException();
                }

                return Task.FromResult<List<DomainOrder?>?>(_findResult.Cast<DomainOrder?>().ToList());
            }

            public Task<bool> InsertOneAsync(DomainOrder orderEntity, CancellationToken cancellationToken)
            {
                if (_throwOnInsert)
                {
                    throw new InvalidOperationException();
                }

                InsertedOrder = orderEntity;
                return Task.FromResult(_insertResult);
            }
        }

        private sealed class FakeOrderProducer : IOrderProducer
        {
            public List<OrderEnvelope> Envelopes { get; } = new();

            public Task HandleAsync(OrderEnvelope envelope, CancellationToken cancellationToken)
            {
                Envelopes.Add(envelope);
                return Task.CompletedTask;
            }
        }

        private sealed class NullLogger<T> : ILogger<T>
        {
            public static readonly NullLogger<T> Instance = new();

            public IDisposable BeginScope<TState>(TState state) where TState : notnull => NullScope.Instance;

            public bool IsEnabled(LogLevel logLevel) => false;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
            {
            }

            private sealed class NullScope : IDisposable
            {
                public static readonly NullScope Instance = new();

                public void Dispose()
                {
                }
            }
        }
    }
}
