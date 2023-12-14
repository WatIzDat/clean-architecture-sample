using Application.Abstractions.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Infrastructure.Data
{
    internal sealed class NpgsqlConnectionFactory : IDbConnectionFactory
    {
        private readonly IConfiguration configuration;

        public NpgsqlConnectionFactory(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IDbConnection CreateOpenConnection()
        {
            return new NpgsqlConnection(configuration.GetConnectionString("NpgsqlDatabase"));
        }
    }
}
