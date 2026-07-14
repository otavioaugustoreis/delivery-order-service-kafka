using delivery_order_services.Application.Shared.Abstractions.Result;
using delivery_order_services.Controllers.User;
using delivery_order_services.Controllers.User.Model;
using delivery_order_services.Controllers.User.UseCase;
using Microsoft.AspNetCore.Mvc;
using DomainUser = delivery_order_services.Application.Domain.User;

namespace UnitTest.Controllers
{
    public class UserControllertTests
    {
        [Fact]
        public async Task PostCreatingUserAsync_WhenUseCaseSucceeds_ReturnsNoContentResult()
        {
            // Arrange
            var request = new UserRequestModel("Ana", "ana@contoso.com");
            var cancellationToken = new CancellationTokenSource().Token;
            var useCase = new FakeUserUseCase();
            var controller = new UserController(useCase);

            // Act
            var actionResult = await controller.PostCreatingUserAsync(request, cancellationToken);

            // Assert
            Assert.IsType<NoContentResult>(actionResult);
            Assert.Equal(request, useCase.InsertedRequest);
            Assert.Equal(cancellationToken, useCase.InsertedCancellationToken);
        }

        [Fact]
        public async Task GetAllUsersAsync_WhenUseCaseSucceeds_ReturnsOkObjectResultWithUsers()
        {
            // Arrange
            var expectedUsers = new List<DomainUser>
            {
                new() { Name = "Ana", Email = "ana@contoso.com", UserType = "CLIENT" }
            };
            var cancellationToken = CancellationToken.None;
            var useCase = new FakeUserUseCase(expectedUsers);
            var controller = new UserController(useCase);

            // Act
            var actionResult = await controller.GetAllUsersAsync(cancellationToken);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var users = Assert.IsType<List<DomainUser>>(okResult.Value);
            Assert.Single(users);
            Assert.Equal(expectedUsers[0].Name, users[0].Name);
            Assert.Equal(expectedUsers[0].Email, users[0].Email);
            Assert.Equal(expectedUsers[0].UserType, users[0].UserType);
            Assert.Equal(cancellationToken, useCase.FindCancellationToken);
        }

        [Fact]
        public async Task PostCreatingUserAsync_WhenUseCaseFails_ReturnsRequestTimeoutResult()
        {
            // Arrange
            var request = new UserRequestModel("Ana", "ana@contoso.com");
            var useCase = new FakeUserUseCase(Result.Failed(new Error(ErrorCode.RequestTimeout, "timeout")));
            var controller = new UserController(useCase);

            // Act
            var actionResult = await controller.PostCreatingUserAsync(request, CancellationToken.None);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(actionResult);
            Assert.Equal(408, objectResult.StatusCode);
        }

        private sealed class FakeUserUseCase : IUserUseCase
        {
            private readonly Result _insertResult;
            private readonly Result<List<DomainUser>> _findResult;

            public FakeUserUseCase()
                : this(Result.Success(), Result<List<DomainUser>>.Success(new List<DomainUser>()))
            {
            }

            public FakeUserUseCase(List<DomainUser> users)
                : this(Result.Success(), Result<List<DomainUser>>.Success(users))
            {
            }

            public FakeUserUseCase(Result insertResult)
                : this(insertResult, Result<List<DomainUser>>.Success(new List<DomainUser>()))
            {
            }

            private FakeUserUseCase(Result insertResult, Result<List<DomainUser>> findResult)
            {
                _insertResult = insertResult;
                _findResult = findResult;
            }

            public UserRequestModel? InsertedRequest { get; private set; }

            public CancellationToken InsertedCancellationToken { get; private set; }

            public CancellationToken FindCancellationToken { get; private set; }

            public Task<Result> InsertOneAsync(UserRequestModel input, CancellationToken cancellationToken)
            {
                InsertedRequest = input;
                InsertedCancellationToken = cancellationToken;

                return Task.FromResult(_insertResult);
            }

            public Task<Result<List<DomainUser>>> FindAsync(CancellationToken cancellationToken)
            {
                FindCancellationToken = cancellationToken;

                return Task.FromResult(_findResult);
            }
        }
    }
}
