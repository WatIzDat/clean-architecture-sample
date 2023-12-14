using Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interceptors
{
    public sealed class ConvertDomainEventsToOutboxMessagesInterceptor
        : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            DbContext? dbContext = eventData.Context;

            if (dbContext == null)
            {
                return base.SavingChangesAsync(eventData, result, cancellationToken);
            }

            List<OutboxMessage> outboxMessages = dbContext.ChangeTracker
                .Entries<Entity>()
                .Select(e => e.Entity)
                .SelectMany(e =>
                {
                    List<IDomainEvent> domainEvents = e.DomainEvents;

                    e.ClearDomainEvents();

                    return domainEvents;
                })
                .Select(domainEvent => new OutboxMessage
                {
                    Id = Guid.NewGuid(),
                    OccurredOnUtc = DateTime.UtcNow,
                    Type = domainEvent.GetType().Name,
                    Content = JsonConvert.SerializeObject(
                        domainEvent,
                        new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.All
                        })
                })
                .ToList();

            dbContext.Set<OutboxMessage>().AddRange(outboxMessages);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
