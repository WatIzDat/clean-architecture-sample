using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EntityConfigurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Ignore(u => u.DomainEvents);

            builder.ToTable("users");

            builder.Property(u => u.Id).HasColumnName("id");
            builder.OwnsOne(u => u.Email).Property(e => e.Value).HasColumnName("email");
            builder.OwnsOne(u => u.Name).Property(n => n.Value).HasColumnName("name");
            builder.Property(u => u.HasPublicProfile).HasColumnName("has_public_profile");
        }
    }
}
