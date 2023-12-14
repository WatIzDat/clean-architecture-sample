using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.GetByEmail
{
    public sealed record GetUserByEmailQuery(string Email) : IQuery<UserResponse>;
}
