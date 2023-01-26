using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrustNetwork.Domain.Entities;
using TrustNetwork.Infrastructure.Context.ManyToManyEntities;

namespace TrustNetwork.Infrastructure.Context.Configuration
{
    internal class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasKey(person => person.Id);
            builder.Property(person => person.Id)
                .UseIdentityColumn();

            builder.HasIndex(person => person.Login)
                .IsUnique();

            builder.HasMany(person => person.PeopleSendTo)
                .WithOne(relation => relation.Sender)
                .HasForeignKey(relation => relation.SenderId);

            builder.HasMany(person => person.PeopleReceiveFrom)
                .WithOne(relation => relation.Receiver)
                .HasForeignKey(relation => relation.ReceiverId)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasMany(person => person.Topics)
                .WithMany(topic => topic.Persons)
                .UsingEntity<PersonTopicRelation>(
                    nameof(PersonTopicRelation),
                    conf => conf.HasOne(relation => relation.Topic)
                                    .WithMany()
                                    .OnDelete(DeleteBehavior.ClientCascade),
                    conf => conf.HasOne(relation => relation.Person)
                                    .WithMany()
                                    .OnDelete(DeleteBehavior.Cascade));
        }
    }
}
