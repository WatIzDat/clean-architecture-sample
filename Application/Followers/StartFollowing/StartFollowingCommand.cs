using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Followers.StartFollowing
{
    public sealed record StartFollowingCommand(Guid UserId, Guid FollowedId) : ICommand;
}
