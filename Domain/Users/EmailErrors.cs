using SharedKernel;

namespace Domain.Users
{
    public static class EmailErrors
    {
        public static readonly Error Empty = new("Email.Empty", "Email was empty.");
        public static readonly Error InvalidFormat = new("Email.InvalidFormat", "Email format was invalid.");
    }
}