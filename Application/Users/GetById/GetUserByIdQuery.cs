using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.GetById
{
    public sealed record GetUserByIdQuery(Guid UserId) : IQuery<UserResponse>;
}
