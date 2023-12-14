using Application.Abstractions.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    internal sealed class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext dbContext;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
