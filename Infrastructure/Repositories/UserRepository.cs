using Application.Abstractions.Data;
using Application.Users;
using Dapper;
using Domain.Users;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Infrastructure.Repositories
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory connectionFactory;
        private readonly ApplicationDbContext dbContext;

        public UserRepository(IDbConnectionFactory connectionFactory, ApplicationDbContext dbContext)
        {
            this.connectionFactory = connectionFactory;
            this.dbContext = dbContext;
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            //using IDbConnection connection = connectionFactory.CreateOpenConnection();

            //const string sql =
            //    """
            //    SELECT u.Id, u.Email, u.Name, u.has_public_profile
            //    FROM Users u
            //    WHERE u.Id = @Id
            //    """;

            //UserResponse? user = await connection.QueryFirstOrDefaultAsync<UserResponse>(sql, new { Id = id });

            //if (userResponse == null)
            //{
            //    return null;
            //}

            //User user = User.Create(
            //    new Name(userResponse.Name),
            //    Email.Create(userResponse.Email).Value,
            //    userResponse.HasPublicProfile); 

            User? user = await dbContext.Users
                .AsNoTracking()
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            return user;
        }

        public void Insert(User user)
        {
            dbContext.Set<User>().Add(user);
        }

        public async Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default)
        {
            using IDbConnection connection = connectionFactory.CreateOpenConnection();

            const string sql =
                """
                SELECT EXISTS (SELECT 1 FROM Users WHERE Email = @Email)
                """;

            bool exists = await connection.QueryFirstOrDefaultAsync<bool>(sql, new { Email = email.Value });

            // Unique means not existing, so negation is done
            return !exists;
        }
    }
}
