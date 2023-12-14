namespace Application.Abstractions.UserNotifications
{
    public interface IUserNotificationService
    {
        Task SendAsync(Guid userId, string message, CancellationToken cancellationToken = default);
    }
}
