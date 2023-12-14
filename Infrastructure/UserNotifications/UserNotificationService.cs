using Application.Abstractions.UserNotifications;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UserNotifications
{
    internal sealed class UserNotificationService : IUserNotificationService
    {
        private readonly ILogger<UserNotificationService> logger;

        public UserNotificationService(ILogger<UserNotificationService> logger)
        {
            this.logger = logger;
        }

        public Task SendAsync(Guid userId, string message, CancellationToken cancellationToken = default)
        {
            logger.LogInformation("{userId}: {message}", userId, message);

            return Task.CompletedTask;
        }
    }
}
