using Domain.Followers;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.UnitTests.Followers
{
    public class FollowerTests
    {
        [Fact]
        public void Create_Should_ReturnFollower_WhenGuidsAreValid()
        {
            Follower follower = Follower.Create(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow);

            follower.Should().NotBeNull();
        }

        [Fact]
        public void Create_Should_RaiseDomainEvent_WhenGuidsAreValid()
        {
            Follower follower = Follower.Create(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow);

            follower.DomainEvents.Should().ContainSingle()
                .Which.Should().BeOfType<FollowerCreatedDomainEvent>();
        }
    }
}
