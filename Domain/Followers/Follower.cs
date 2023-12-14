using Domain.Users;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Followers
{
    public sealed class Follower : Entity
    {
        private Follower(Guid userId, Guid followedId, DateTime createdOnUtc)
        {
            UserId = userId;
            FollowedId = followedId;
            CreatedOnUtc = createdOnUtc;
        }

        private Follower()
        {
        }

        public Guid UserId { get; private set; }
        public Guid FollowedId { get; private set; }
        public User User { get; private set; } = null!;
        public User Followed { get; private set; } = null!;

        public DateTime CreatedOnUtc { get; private set; }

        public static Follower Create(Guid userId, Guid followedId, DateTime createdOnUtc)
        {
            Follower follower = new(userId, followedId, createdOnUtc);

            follower.Raise(new FollowerCreatedDomainEvent(follower.UserId, follower.FollowedId));

            return follower;
        }
    }
}
