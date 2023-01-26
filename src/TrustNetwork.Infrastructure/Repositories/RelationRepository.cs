using Microsoft.EntityFrameworkCore;
using TrustNetwork.Application.Repositories;
using TrustNetwork.Domain.Entities;
using TrustNetwork.Infrastructure.Context;

namespace TrustNetwork.Infrastructure.Repositories
{
    public class RelationRepository : Repository<Relation>, IRepository<Relation>, IRelationsRepository
    {
        private readonly TrustNetworkDbContext _context;
        public RelationRepository(TrustNetworkDbContext context) : base(context) => _context = context;

        public async Task<Relation?> GetRelationByPersonsOrDefaultAsync(int senderId, int receiverId)
            => await _context.Relations.SingleOrDefaultAsync(
                        relation => relation.SenderId == senderId && relation.ReceiverId == receiverId);

        public async Task<Relation> CreateOrUpdateRelationsAsync(Person sender, Person receiver, int trustLevel)
        {
            var relation = await GetRelationByPersonsOrDefaultAsync(sender.Id, receiver.Id);

            if (relation is not null)
            {
                relation.TrustLevel = trustLevel;
                return relation;
            }

            relation = new Relation
            {
                Receiver = receiver,
                Sender = sender,
                TrustLevel = trustLevel
            };

            Add(relation);

            return relation;
        }
    }
}
