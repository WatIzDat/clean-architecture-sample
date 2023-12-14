using Domain.Users;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.UnitTests.Users
{
    public class UserTests
    {
        [Fact]
        public void Create_Should_ReturnUser_WhenParametersAreValid()
        {
            // Arrange
            Name name = new("Full name");
            Email email = Email.Create("test@test.com").Value;

            // Act
            User user = User.Create(name, email, false);

            // Assert
            user.Should().NotBeNull();
        }

        [Fact]
        public void Create_Should_RaiseDomainEvent_WhenParametersAreValid()
        {
            // Arrange
            Name name = new("Full name");
            Email email = Email.Create("test@test.com").Value;

            // Act
            User user = User.Create(name, email, false);

            // Assert
            user.DomainEvents.Should().ContainSingle()
                .Which.Should().BeOfType<UserCreatedDomainEvent>();
        }
    }
}
