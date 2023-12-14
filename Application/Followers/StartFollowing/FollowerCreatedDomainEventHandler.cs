using Application.Abstractions.UserNotifications;
using Domain.Followers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Followers.StartFollowing
{
    internal sealed class FollowerCreatedDomainEventHandler : INotificationHandler<FollowerCreatedDomainEvent>
    {
        private readonly IUserNotificationService notificationService;

        public FollowerCreatedDomainEventHandler(IUserNotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        public async Task Handle(FollowerCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            await notificationService.SendAsync(
                notification.FollowerId,
                "You just got a new follower!",
                cancellationToken);
        }
    }
}
