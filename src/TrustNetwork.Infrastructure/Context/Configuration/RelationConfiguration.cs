using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrustNetwork.Domain.Entities;

namespace TrustNetwork.Infrastructure.Context.Configuration
{
    internal class RelationConfiguration : IEntityTypeConfiguration<Relation>
    {
        public void Configure(EntityTypeBuilder<Relation> builder)
        {
            builder.HasKey(relation => relation.Id);
            builder.Property(relation => relation.Id)
                .UseIdentityColumn();

            builder.HasIndex(relation => new { relation.SenderId, relation.ReceiverId })
                .IsUnique();

            builder.HasOne(relation => relation.Sender)
                .WithMany(people => people.PeopleSendTo)
                .HasForeignKey(relation => relation.SenderId);

            builder.HasOne(relation => relation.Receiver)
                .WithMany(people => people.PeopleReceiveFrom)
                .HasForeignKey(relation => relation.ReceiverId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
