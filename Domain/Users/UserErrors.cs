using SharedKernel;

namespace Domain.Users
{
    public static class UserErrors
    {
        public static Error NotFound(Guid userId)
        {
            return new("Users.NotFound", $"The user with the Id '{userId}' was not found.");
        }
        
        public static Error NotFoundByEmail(string email)
        {
            return new("Users.NotFoundByEmail", $"The user with the email '{email}' was not found.");
        }

        public static Error EmailNotUnique(string email)
        {
            return new("Users.EmailNotUnique", $"The email '{email}' already exists.");
        }
    }
}
