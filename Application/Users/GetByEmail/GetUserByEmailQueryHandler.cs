using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Dapper;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System.Data;

namespace Application.Users.GetByEmail
{
    internal sealed class GetUserByEmailQueryHandler : IQueryHandler<GetUserByEmailQuery, UserResponse>
    {
        private readonly IDbConnectionFactory connectionFactory;

        public GetUserByEmailQueryHandler(IDbConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task<Result<UserResponse>> Handle(GetUserByEmailQuery query, CancellationToken cancellationToken)
        {
            //UserResponse? user = await dbContext.Users
            //    .AsNoTracking()
            //    .Where(u => u.Email.Value == query.Email)
            //    .Select(u => new UserResponse
            //    {
            //        Id = u.Id,
            //        Email = u.Email.Value!,
            //        Name = u.Name.Value!,
            //        HasPublicProfile = u.HasPublicProfile
            //    })
            //    .FirstOrDefaultAsync(cancellationToken);

            using IDbConnection connection = connectionFactory.CreateOpenConnection();

            const string sql =
                """
                SELECT u.Id, u.Email, u.Name, u.has_public_profile
                FROM Users u
                WHERE u.Email = @Email
                """;

            UserResponse? user = await connection.QueryFirstOrDefaultAsync<UserResponse>(sql, new { query.Email });
            
            if (user == null)
            {
                return Result.Failure<UserResponse>(UserErrors.NotFoundByEmail(query.Email));
            }

            return Result.Success(user);
        }
    }
}
