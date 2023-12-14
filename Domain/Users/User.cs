using Domain.Followers;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Users
{
    public sealed class User : Entity
    {
        private User(Guid id, Name name, Email email, bool hasPublicProfile)
            : base(id)
        {
            Name = name;
            Email = email;
            HasPublicProfile = hasPublicProfile;
        }

        private User()
        {
        }

        public Name Name { get; private set; } = null!;
        public Email Email { get; private set; } = null!;
        public bool HasPublicProfile { get; set; }
        public ICollection<Follower> Followers { get; private set; } = null!;
        public ICollection<Follower> Following { get; private set; } = null!;

        public static User Create(Name name, Email email, bool hasPublicProfile)
        {
            User user = new(Guid.NewGuid(), name, email, hasPublicProfile);

            user.Raise(new UserCreatedDomainEvent(user.Id));

            return user;
        }
    }
}
