using Domain;
using Domain.Exceptions;
using Infrastructure.Data;
using Infrastructure.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.BackgroundJobs
{
    [DisallowConcurrentExecution]
    public class ProcessOutboxMessagesJob : IJob
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IPublisher publisher;

        public ProcessOutboxMessagesJob(IPublisher publisher, ApplicationDbContext dbContext)
        {
            this.publisher = publisher;
            this.dbContext = dbContext;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                List<OutboxMessage> outboxMessages = await dbContext
                                .Set<OutboxMessage>()
                                .Where(o => o.ProcessedOnUtc == null)
                                .Take(20)
                                .ToListAsync(context.CancellationToken);

                foreach (OutboxMessage outboxMessage in outboxMessages)
                {
                    IDomainEvent? domainEvent = JsonConvert
                        .DeserializeObject<IDomainEvent>(
                            outboxMessage.Content,
                            new JsonSerializerSettings
                            {
                                TypeNameHandling = TypeNameHandling.All
                            });

                    if (domainEvent == null)
                    {
                        throw new DeserializationNullException($"{nameof(domainEvent)} was null.");
                    }

                    await publisher.Publish(domainEvent, context.CancellationToken);

                    outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
                }

                await dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new OutboxMessageProcessingFailedException(
                    "The outbox message failed to be processed.", e);
            }
        }
    }
}
