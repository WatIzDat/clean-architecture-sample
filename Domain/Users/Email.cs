using SharedKernel;

namespace Domain.Users
{
    public sealed record Email
    {
        private Email(string? value)
        {
            Ensure.NotNullOrEmpty(value);

            Value = value;
        }

        public string Value { get; }

        public static Result<Email> Create(string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Result.Failure<Email>(EmailErrors.Empty);
            }

            if (value.Split('@').Length != 2)
            {
                return Result.Failure<Email>(EmailErrors.InvalidFormat);
            }

            return Result.Success(new Email(value));
        }
    }
}