using delivery_order_services.Application.Shared.Abstractions.Result;
using delivery_order_services.Controllers.Order;
using delivery_order_services.Controllers.Order.Models;
using delivery_order_services.Controllers.Order.UseCase;
using delivery_order_services.Helpers;
using Microsoft.AspNetCore.Mvc;
using DomainOrder = delivery_order_services.Application.Domain.Order;

namespace UnitTest.Controllers
{
    public class OrderControllerTests
    {
        [Fact]
        public async Task PostCreateEventOrderAsync_WhenUseCaseSucceeds_ReturnsNoContentResult()
        {
            // Arrange
            var request = new OrderRequestModel("Notebook", "client-1");
            var cancellationToken = new CancellationTokenSource().Token;
            var useCase = new FakeOrderEventUseCase(Result.Success());
            var controller = new OrderController(useCase);

            // Act
            var result = await controller.PostCreateEventOrderAsync(request, "idem-1", cancellationToken);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(request, useCase.InsertedRequest);
            Assert.Equal("idem-1", useCase.InsertedIdempotencyKey);
            Assert.Equal(cancellationToken, useCase.InsertedCancellationToken);
        }

        [Fact]
        public async Task PostCreateEventOrderAsync_WhenUseCaseFailsWithConflict_ReturnsConflictResult()
        {
            // Arrange
            var request = new OrderRequestModel("Notebook", "client-1");
            var useCase = new FakeOrderEventUseCase(Result.Failed(new Error(ErrorCode.Conflict, "duplicated")));
            var controller = new OrderController(useCase);

            // Act
            var result = await controller.PostCreateEventOrderAsync(request, "idem-1", CancellationToken.None);

            // Assert
            Assert.IsType<ConflictResult>(result);
        }

        [Fact]
        public async Task GetOrdersByClientIdAsync_WhenUseCaseSucceeds_ReturnsOkObjectResultWithOrders()
        {
            // Arrange
            var expectedOrders = new List<DomainOrder>
            {
                new() { ProductName = "Notebook", ClientId = "client-1", OrderStatus = "CREATED", IdempotencyKey = "idem-1" }
            };
            var useCase = new FakeOrderEventUseCase(expectedOrders);
            var controller = new OrderController(useCase);

            // Act
            var result = await controller.GetOrdersByClientIdAsync("client-1", CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var orders = Assert.IsType<List<DomainOrder>>(okResult.Value);
            Assert.Single(orders);
            Assert.Equal("Notebook", orders[0].ProductName);
            Assert.Equal("client-1", orders[0].ClientId);
            Assert.Equal("CREATED", orders[0].OrderStatus);
            Assert.Equal("idem-1", orders[0].IdempotencyKey);
            Assert.Equal("client-1", useCase.FindClientId);
        }

        [Fact]
        public async Task GetOrdersByClientIdAsync_WhenUseCaseFails_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var useCase = new FakeOrderEventUseCase(Result<List<DomainOrder>>.Failed(new Error(ErrorCode.UnexpectedError, "boom")));
            var controller = new OrderController(useCase);

            // Act
            var result = await controller.GetOrdersByClientIdAsync("client-1", CancellationToken.None);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("boom", badRequest.Value);
        }

        private sealed class FakeOrderEventUseCase : IOrderEventUseCase
        {
            private readonly Result _insertResult;
            private readonly Result<List<DomainOrder>> _findResult;

            public FakeOrderEventUseCase(Result insertResult)
                : this(insertResult, Result<List<DomainOrder>>.Success(new List<DomainOrder>()))
            {
            }

            public FakeOrderEventUseCase(List<DomainOrder> orders)
                : this(Result.Success(), Result<List<DomainOrder>>.Success(orders))
            {
            }

            public FakeOrderEventUseCase(Result<List<DomainOrder>> findResult)
                : this(Result.Success(), findResult)
            {
            }

            private FakeOrderEventUseCase(Result insertResult, Result<List<DomainOrder>> findResult)
            {
                _insertResult = insertResult;
                _findResult = findResult;
            }

            public OrderRequestModel? InsertedRequest { get; private set; }

            public string? InsertedIdempotencyKey { get; private set; }

            public CancellationToken InsertedCancellationToken { get; private set; }

            public string? FindClientId { get; private set; }

            public Task<Result> InsertOneAsync(OrderRequestModel input, string? idempotencyKey, CancellationToken cancellationToken)
            {
                InsertedRequest = input;
                InsertedIdempotencyKey = idempotencyKey;
                InsertedCancellationToken = cancellationToken;

                return Task.FromResult(_insertResult);
            }

            public Task<Result<List<DomainOrder>>> FindByClientIdAsync(string clientId, CancellationToken cancellationToken)
            {
                FindClientId = clientId;

                return Task.FromResult(_findResult);
            }
        }
    }
}
