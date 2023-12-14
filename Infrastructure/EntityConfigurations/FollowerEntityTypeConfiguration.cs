using Domain.Followers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EntityConfigurations
{
    public class FollowerEntityTypeConfiguration : IEntityTypeConfiguration<Follower>
    {
        public void Configure(EntityTypeBuilder<Follower> builder)
        {
            builder.HasKey(f => new { f.UserId, f.FollowedId });
            builder.Ignore(f => f.DomainEvents);
            builder.Ignore(f => f.Id);

            builder.ToTable("followers");

            builder.Property(f => f.UserId).HasColumnName("user_id");
            builder.Property(f => f.FollowedId).HasColumnName("followed_id");
            builder.Property(f => f.CreatedOnUtc).HasColumnName("created_on_utc");

            builder.HasOne(f => f.User).WithMany(u => u.Following).HasForeignKey(f => f.UserId);
            builder.HasOne(f => f.Followed).WithMany(u => u.Followers).HasForeignKey(f => f.FollowedId);
        }
    }
}
