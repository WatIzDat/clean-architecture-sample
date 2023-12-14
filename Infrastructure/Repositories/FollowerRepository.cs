using Application.Abstractions.Data;
using Dapper;
using Domain.Followers;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public sealed class FollowerRepository : IFollowerRepository
    {
        private readonly IDbConnectionFactory connectionFactory;
        private readonly ApplicationDbContext dbContext;

        public FollowerRepository(IDbConnectionFactory connectionFactory, ApplicationDbContext dbContext)
        {
            this.connectionFactory = connectionFactory;
            this.dbContext = dbContext;
        }

        public void Insert(Follower follower)
        {
            dbContext.Set<Follower>().Add(follower);
        }

        public async Task<bool> IsAlreadyFollowingAsync(Guid userId, Guid followedId, CancellationToken cancellationToken = default)
        {
            using IDbConnection connection = connectionFactory.CreateOpenConnection();

            const string sql =
                """
                SELECT EXISTS (SELECT 1 FROM followers WHERE user_id = @UserId AND followed_id = @FollowedId)
                """;

            bool exists = await connection.QueryFirstOrDefaultAsync<bool>(sql,
                new { UserId = userId, FollowedId = followedId });

            return exists;
        }
    }
}
