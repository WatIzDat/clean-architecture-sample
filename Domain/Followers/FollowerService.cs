using SharedKernel;
using Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Followers
{
    public sealed class FollowerService : IFollowerService
    {
        private readonly IFollowerRepository followerRepository;
        private readonly IDateTimeProvider dateTimeProvider;

        public FollowerService(IFollowerRepository followerRepository, IDateTimeProvider dateTimeProvider)
        {
            this.followerRepository = followerRepository;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result> StartFollowingAsync(
            User user,
            User followed,
            CancellationToken cancellationToken)
        {
            if (user.Id == followed.Id)
            {
                return Result.Failure(FollowerErrors.SameUser);
            }

            if (!followed.HasPublicProfile)
            {
                return Result.Failure(FollowerErrors.NonPublicProfile);
            }

            if (await followerRepository.IsAlreadyFollowingAsync(
                    user.Id,
                    followed.Id,
                    cancellationToken))
            {
                return Result.Failure(FollowerErrors.AlreadyFollowing);
            }

            Follower follower = Follower.Create(user.Id, followed.Id, dateTimeProvider.UtcNow);

            followerRepository.Insert(follower);

            return Result.Success();
        }
    }
}
