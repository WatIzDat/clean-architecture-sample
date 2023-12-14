namespace Domain.Followers
{
    public interface IFollowerRepository
    {
        void Insert(Follower follower);
        Task<bool> IsAlreadyFollowingAsync(Guid userId, Guid followedId, CancellationToken cancellationToken = default);
    }
}
