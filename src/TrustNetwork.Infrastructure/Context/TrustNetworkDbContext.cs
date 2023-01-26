using Microsoft.EntityFrameworkCore;
using TrustNetwork.Domain.Entities;
using TrustNetwork.Infrastructure.Context.Configuration;

namespace TrustNetwork.Infrastructure.Context
{
    public class TrustNetworkDbContext : DbContext
    {
        public DbSet<Person> People { get; set; } = null!;
        public DbSet<Topic> Topics { get; set; } = null!;
        public DbSet<Relation> Relations { get; set; } = null!;

        public TrustNetworkDbContext(DbContextOptions opt) : base(opt) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PersonConfiguration());

            modelBuilder.ApplyConfiguration(new TopicConfiguration());

            modelBuilder.ApplyConfiguration(new RelationConfiguration());
        }
    }
}
