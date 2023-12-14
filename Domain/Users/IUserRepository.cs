namespace Domain.Users
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        void Insert(User user);
        Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default);
    }
}
