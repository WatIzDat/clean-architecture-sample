using Application.Abstractions.Data;
using Domain.Followers;
using Domain.Users;
using Infrastructure.EntityConfigurations;
using Infrastructure.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly IPublisher publisher;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IPublisher publisher)
            : base(options)
        {
            this.publisher = publisher;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Follower> Followers { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FollowerEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
        }

        //public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        //{
        //    int result = await base.SaveChangesAsync(cancellationToken);

        //    await PublishDomainEventsAsync();

        //    return result;
        //}

        //private async Task PublishDomainEventsAsync()
        //{
        //    List<IDomainEvent> domainEvents = ChangeTracker
        //        .Entries<Entity>()
        //        .Select(e => e.Entity)
        //        .SelectMany(e =>
        //        {
        //            List<IDomainEvent> domainEvents = e.DomainEvents;

        //            e.ClearDomainEvents();

        //            return domainEvents;
        //        })
        //        .ToList();

        //    foreach (IDomainEvent domainEvent in domainEvents)
        //    {
        //        await publisher.Publish(domainEvent);
        //    }
        //}
    }
}
