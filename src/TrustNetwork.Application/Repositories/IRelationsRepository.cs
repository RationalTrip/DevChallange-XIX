using TrustNetwork.Domain.Entities;

namespace TrustNetwork.Application.Repositories
{
    public interface IRelationsRepository : IRepository<Relation>
    {
        Task<Relation> CreateOrUpdateRelationsAsync(Person sender, Person receiver, int trustLevel);
        Task<Relation?> GetRelationByPersonsOrDefaultAsync(int senderId, int receiverId);
    }
}
