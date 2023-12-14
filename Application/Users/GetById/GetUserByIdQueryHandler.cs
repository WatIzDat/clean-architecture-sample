using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.GetById
{
    internal sealed class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserResponse>
    {
        private readonly IApplicationDbContext dbContext;

        public GetUserByIdQueryHandler(IApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Result<UserResponse>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
        {
            UserResponse? user = await dbContext.Users
                .AsNoTracking()
                .Where(u => u.Id == query.UserId)
                .Select(u => new UserResponse
                {
                    Id = u.Id,
                    Email = u.Email.Value!,
                    Name = u.Name.Value!,
                    HasPublicProfile = u.HasPublicProfile
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                return Result.Failure<UserResponse>(UserErrors.NotFound(query.UserId));
            }

            return Result.Success(user);
        }
    }
}
