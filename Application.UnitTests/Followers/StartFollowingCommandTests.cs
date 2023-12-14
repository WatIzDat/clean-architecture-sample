using Application.Abstractions.Data;
using Application.Followers.StartFollowing;
using Domain.Followers;
using Domain.Users;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTests.Followers
{
    public class StartFollowingCommandTests
    {
        private static readonly User User = User.Create(
            new Name("Full name"),
            Email.Create("test@test.com").Value,
            true);

        private static readonly StartFollowingCommand Command = new(Guid.NewGuid(), Guid.NewGuid());

        private readonly StartFollowingCommandHandler handler;

        private readonly IUserRepository userRepositoryMock;
        private readonly IUnitOfWork unitOfWorkMock;
        private readonly IFollowerService followerServiceMock;

        public StartFollowingCommandTests()
        {
            userRepositoryMock = Substitute.For<IUserRepository>();
            unitOfWorkMock = Substitute.For<IUnitOfWork>();
            followerServiceMock = Substitute.For<IFollowerService>();

            handler = new StartFollowingCommandHandler(userRepositoryMock, followerServiceMock, unitOfWorkMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenUserCouldNotBeFound()
        {
            // Arrange
            userRepositoryMock.GetByIdAsync(Arg.Is<Guid>(id => id == Command.UserId))
                .ReturnsNull();

            // Act
            Result result = await handler.Handle(Command, default);

            // Assert
            result.Error.Should().Be(UserErrors.NotFound(Command.UserId));
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenFollowedCouldNotBeFound()
        {
            // Arrange
            userRepositoryMock.GetByIdAsync(Arg.Is<Guid>(id => id == Command.UserId))
                .Returns(User);

            userRepositoryMock.GetByIdAsync(Arg.Is<Guid>(id => id == Command.FollowedId))
                .ReturnsNull();

            // Act
            Result result = await handler.Handle(Command, default);

            // Assert
            result.Error.Should().Be(UserErrors.NotFound(Command.FollowedId));
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenStartFollowingAsyncFails()
        {
            // Arrange
            userRepositoryMock.GetByIdAsync(Arg.Is<Guid>(id => id == Command.UserId))
                .Returns(User);

            userRepositoryMock.GetByIdAsync(Arg.Is<Guid>(id => id == Command.FollowedId))
                .Returns(User);

            followerServiceMock.StartFollowingAsync(
                Arg.Is<User>(u => u.Id == User.Id),
                Arg.Is<User>(u => u.Id == User.Id),
                default)
                .Returns(Result.Failure(FollowerErrors.SameUser));

            // Act
            Result result = await handler.Handle(Command, default);

            // Assert
            result.Error.Should().Be(FollowerErrors.SameUser);
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenParametersAreValid()
        {
            // Arrange
            userRepositoryMock.GetByIdAsync(Arg.Is<Guid>(id => id == Command.UserId))
                .Returns(User);

            userRepositoryMock.GetByIdAsync(Arg.Is<Guid>(id => id == Command.FollowedId))
                .Returns(User);

            followerServiceMock.StartFollowingAsync(
                Arg.Is<User>(u => u.Id == User.Id),
                Arg.Is<User>(u => u.Id == User.Id),
                default)
                .Returns(Result.Success());

            // Act
            Result result = await handler.Handle(Command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_CallUnitOfWork_WhenParametersAreValid()
        {
            // Arrange
            userRepositoryMock.GetByIdAsync(Arg.Is<Guid>(id => id == Command.UserId))
                .Returns(User);

            userRepositoryMock.GetByIdAsync(Arg.Is<Guid>(id => id == Command.FollowedId))
                .Returns(User);

            followerServiceMock.StartFollowingAsync(
                Arg.Is<User>(u => u.Id == User.Id),
                Arg.Is<User>(u => u.Id == User.Id),
                default)
                .Returns(Result.Success());

            // Act
            Result result = await handler.Handle(Command, default);

            // Assert
            await unitOfWorkMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}
