using delivery_order_services.Controllers.User.Model;
using delivery_order_services.Controllers.User.UseCase;
using Microsoft.Extensions.Logging.Abstractions;
using DomainUser = delivery_order_services.Application.Domain.User;

namespace UnitTest.UseCase
{
    public class UseUseCaseTests
    {
        [Fact]
        public async Task InsertOneAsync_WhenRequestIsValid_MapsToEntitySetsClientAndInserts()
        {
            // Arrange
            var request = new UserRequestModel("Ana", "ana@contoso.com");
            var repository = new FakeUserRepository();
            var useCase = new UserUseCase(repository, NullLogger<UserUseCase>.Instance);

            // Act
            var result = await useCase.InsertOneAsync(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(repository.InsertedUser);
            Assert.Equal(request.Name, repository.InsertedUser!.Name);
            Assert.Equal(request.Email, repository.InsertedUser.Email);
            Assert.Equal("CLIENT", repository.InsertedUser.UserType);
        }

        [Fact]
        public async Task InsertOneAsync_WhenRepositoryThrows_ReturnsFailedResult()
        {
            // Arrange
            var request = new UserRequestModel("Ana", "ana@contoso.com");
            var repository = new FakeUserRepository(throwOnInsert: true);
            var useCase = new UserUseCase(repository, NullLogger<UserUseCase>.Instance);

            // Act
            var result = await useCase.InsertOneAsync(request, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("An error occurred in the class UserUseCase", result.Error?.ErrorMessage);
        }

        [Fact]
        public async Task FindAsync_WhenRepositoryReturnsUsers_ReturnsSuccessWithUsers()
        {
            // Arrange
            var expectedUsers = new List<DomainUser>
            {
                new() { Name = "Ana", Email = "ana@contoso.com", UserType = "CLIENT" }
            };
            var repository = new FakeUserRepository(expectedUsers);
            var useCase = new UserUseCase(repository, NullLogger<UserUseCase>.Instance);

            // Act
            var result = await useCase.FindAsync(CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            var users = Assert.IsType<List<DomainUser>>(result.GetContent());
            Assert.Single(users);
            Assert.Equal(expectedUsers[0].Name, users[0].Name);
            Assert.Equal(expectedUsers[0].Email, users[0].Email);
            Assert.Equal(expectedUsers[0].UserType, users[0].UserType);
        }

        [Fact]
        public async Task FindAsync_WhenRepositoryThrows_ReturnsFailedResult()
        {
            // Arrange
            var repository = new FakeUserRepository(throwOnFind: true);
            var useCase = new UserUseCase(repository, NullLogger<UserUseCase>.Instance);

            // Act
            var result = await useCase.FindAsync(CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("An error occurred in the class UserUseCase", result.Error?.ErrorMessage);
        }

        private sealed class FakeUserRepository : delivery_order_services.Application.Shared.Infra.Repositories.User.IUserRepository
        {
            private readonly List<delivery_order_services.Application.Domain.User> _users;
            private readonly bool _throwOnInsert;
            private readonly bool _throwOnFind;

            public FakeUserRepository(List<delivery_order_services.Application.Domain.User>? users = null, bool throwOnInsert = false, bool throwOnFind = false)
            {
                _users = users ?? new List<delivery_order_services.Application.Domain.User>();
                _throwOnInsert = throwOnInsert;
                _throwOnFind = throwOnFind;
            }

            public delivery_order_services.Application.Domain.User? InsertedUser { get; private set; }

            public Task<List<delivery_order_services.Application.Domain.User>> FindAsync(CancellationToken cancellationToken)
            {
                if (_throwOnFind)
                {
                    throw new InvalidOperationException();
                }

                return Task.FromResult(_users);
            }

            public Task<delivery_order_services.Application.Domain.User?> FindByIdAsync(string id, CancellationToken cancellationToken)
            {
                return Task.FromResult<delivery_order_services.Application.Domain.User?>(null);
            }

            public Task InsertOneAsync(delivery_order_services.Application.Domain.User userEntity, CancellationToken cancellationToken)
            {
                if (_throwOnInsert)
                {
                    throw new InvalidOperationException();
                }

                InsertedUser = userEntity;
                return Task.CompletedTask;
            }
        }
    }
}
