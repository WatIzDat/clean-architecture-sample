using Application.Abstractions.Data;
using Application.Users.Create;
using Domain.Users;
using FluentAssertions;
using NSubstitute;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTests.Users
{
    public class CreateUserCommandTests
    {
        private static readonly CreateUserCommand Command = 
            new("test@test.com", "Full name", true);

        private readonly CreateUserCommandHandler handler;
        private readonly IUserRepository userRepositoryMock;
        private readonly IUnitOfWork unitOfWorkMock;

        public CreateUserCommandTests()
        {
            userRepositoryMock = Substitute.For<IUserRepository>();
            unitOfWorkMock = Substitute.For<IUnitOfWork>();

            handler = new CreateUserCommandHandler(userRepositoryMock, unitOfWorkMock);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenEmailHasInvalidFormat()
        {
            // Arrange
            CreateUserCommand invalidCommand = Command with { Email = "thisisaninvalidemail" };

            // Act
            Result result = await handler.Handle(invalidCommand, default);

            // Assert
            result.Error.Should().Be(EmailErrors.InvalidFormat);
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenEmailIsNotUnique()
        {
            // Arrange
            userRepositoryMock.IsEmailUniqueAsync(Arg.Is<Email>(e => e.Value == Command.Email), default)
                .Returns(false);

            // Act
            Result result = await handler.Handle(Command, default);

            // Assert
            result.Error.Should().Be(UserErrors.EmailNotUnique(Command.Email));
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccess_WhenEmailIsValidAndUnique()
        {
            // Arrange
            userRepositoryMock.IsEmailUniqueAsync(Arg.Is<Email>(e => e.Value == Command.Email), default)
                .Returns(true);

            // Act
            Result result = await handler.Handle(Command, default);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_Should_CallUnitOfWork_WhenEmailIsValidAndUnique()
        {
            // Arrange
            userRepositoryMock.IsEmailUniqueAsync(Arg.Is<Email>(e => e.Value == Command.Email), default)
                .Returns(true);

            // Act
            Result result = await handler.Handle(Command, default);

            // Assert
            await unitOfWorkMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_Should_CallUserRepository_WhenEmailIsValidAndUnique()
        {
            // Arrange
            userRepositoryMock.IsEmailUniqueAsync(Arg.Is<Email>(e => e.Value == Command.Email), default)
                .Returns(true);

            // Act
            Result<Guid> result = await handler.Handle(Command, default);

            // Assert
            userRepositoryMock
                .Received(1)
                .Insert(Arg.Is<User>(u => u.Id == result.Value &&
                                          u.Email.Value == Command.Email &&
                                          u.Name.Value == Command.Name &&
                                          u.HasPublicProfile == Command.HasPublicProfile));
        }
    }
}
