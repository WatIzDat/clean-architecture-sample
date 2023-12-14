namespace Application.Users
{
    public sealed record UserResponse
    {
        public Guid Id { get; init; }
        public required string Email { get; init; }
        public required string Name { get; init; }
        public bool HasPublicProfile { get; init; }
    }
}
