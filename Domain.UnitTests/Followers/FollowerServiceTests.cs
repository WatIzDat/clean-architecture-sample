using Domain.Followers;
using Domain.Users;
using FluentAssertions;
using NSubstitute;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.UnitTests.Followers
{
    public class FollowerServiceTests
    {
        private readonly FollowerService followerService;
        private readonly IFollowerRepository followerRepositoryMock;
        private static readonly Name Name = new("Full name");
        private static readonly Email Email = Email.Create("test@test.com").Value;
        private static readonly DateTime UtcNow = DateTime.UtcNow;

        public FollowerServiceTests()
        {
            followerRepositoryMock = Substitute.For<IFollowerRepository>();

            IDateTimeProvider dateTimeProvider = Substitute.For<IDateTimeProvider>();
            dateTimeProvider.UtcNow.Returns(UtcNow);

            followerService = new(followerRepositoryMock, dateTimeProvider);
        }

        [Fact]
        public async Task StartFollowingAsync_Should_ReturnError_WhenFollowingSameUser()
        {
            // Arrange
            User user = User.Create(Name, Email, hasPublicProfile: false);

            // Act
            Result result = await followerService.StartFollowingAsync(user, user, default);

            // Assert
            result.Error.Should().Be(FollowerErrors.SameUser);
        }

        [Fact]
        public async Task StartFollowingAsync_Should_ReturnError_WhenFollowingNonPublicProfile()
        {
            // Arrange
            User user = User.Create(Name, Email, hasPublicProfile: true);
            User followed = User.Create(Name, Email, hasPublicProfile: false);

            // Act
            Result result = await followerService.StartFollowingAsync(user, followed, default);

            // Assert
            result.Error.Should().Be(FollowerErrors.NonPublicProfile);
        }

        [Fact]
        public async Task StartFollowingAsync_Should_ReturnError_WhenAlreadyFollowing()
        {
            // Arrange
            User user = User.Create(Name, Email, hasPublicProfile: true);
            User followed = User.Create(Name, Email, hasPublicProfile: true);

            followerRepositoryMock
                .IsAlreadyFollowingAsync(user.Id, followed.Id, default)
                .Returns(true);

            // Act
            Result result = await followerService.StartFollowingAsync(user, followed, default);

            // Assert
            result.Error.Should().Be(FollowerErrors.AlreadyFollowing);
        }

        [Fact]
        public async Task StartFollowingAsync_Should_ReturnSuccess_WhenFollowerCreated()
        {
            // Arrange
            User user = User.Create(Name, Email, hasPublicProfile: true);
            User followed = User.Create(Name, Email, hasPublicProfile: true);

            followerRepositoryMock
                .IsAlreadyFollowingAsync(user.Id, followed.Id, default)
                .Returns(false);

            // Act
            Result result = await followerService.StartFollowingAsync(user, followed, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task StartFollowingAsync_Should_CallInsertOnRepository_WhenFollowerCreated()
        {
            // Arrange
            User user = User.Create(Name, Email, hasPublicProfile: true);
            User followed = User.Create(Name, Email, hasPublicProfile: true);

            followerRepositoryMock
                .IsAlreadyFollowingAsync(user.Id, followed.Id, default)
                .Returns(false);

            // Act
            await followerService.StartFollowingAsync(user, followed, default);

            // Assert
            followerRepositoryMock.Received(1)
                .Insert(Arg.Is<Follower>(f => f.UserId == user.Id &&
                                              f.FollowedId == followed.Id &&
                                              f.CreatedOnUtc == UtcNow));
        }
    }
}
