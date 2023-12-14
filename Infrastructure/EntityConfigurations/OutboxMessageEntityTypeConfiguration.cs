using Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EntityConfigurations
{
    public class OutboxMessageEntityTypeConfiguration : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.HasKey(o => o.Id);

            builder.ToTable("outbox_messages");

            builder.Property(o => o.Id).HasColumnName("id");
            builder.Property(o => o.Type).HasColumnName("type");
            builder.Property(o => o.Content).HasColumnName("content");
            builder.Property(o => o.OccurredOnUtc).HasColumnName("occurred_on_utc");
            builder.Property(o => o.ProcessedOnUtc).HasColumnName("processed_on_utc");
            builder.Property(o => o.Error).HasColumnName("error");
        }
    }
}
