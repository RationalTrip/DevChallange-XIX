using TrustNetwork.Domain.Entities;

namespace TrustNetwork.Infrastructure.Context.ManyToManyEntities
{
    public class PersonTopicRelation
    {
        public virtual Person Person { get; set; } = null!;
        public virtual Topic Topic { get; set; } = null!;
    }
}
