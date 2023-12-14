using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Create
{
    public sealed record CreateUserCommand(string Email, string Name, bool HasPublicProfile) : ICommand<Guid>;
}
