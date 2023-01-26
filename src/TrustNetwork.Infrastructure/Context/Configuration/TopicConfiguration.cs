using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrustNetwork.Domain.Entities;
using TrustNetwork.Infrastructure.Context.ManyToManyEntities;

namespace TrustNetwork.Infrastructure.Context.Configuration
{
    internal class TopicConfiguration : IEntityTypeConfiguration<Topic>
    {
        public void Configure(EntityTypeBuilder<Topic> builder)
        {
            builder.HasKey(topic => topic.Id);
            builder.Property(topic => topic.Id)
                .UseIdentityColumn();

            builder.HasIndex(topic => topic.Name)
                .IsUnique();

            builder.HasMany(topic => topic.Persons)
                .WithMany(person => person.Topics)
                .UsingEntity<PersonTopicRelation>(
                    nameof(PersonTopicRelation),
                    conf => conf.HasOne(relation => relation.Person)
                                    .WithMany()
                                    .OnDelete(DeleteBehavior.Cascade),
                    conf => conf.HasOne(relation => relation.Topic)
                                    .WithMany()
                                    .OnDelete(DeleteBehavior.ClientCascade));
        }
    }
}
