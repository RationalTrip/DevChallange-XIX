using TrustNetwork.Domain.Entities;

namespace TrustNetwork.Application.Repositories
{
    public interface ITopicsRepository : IRepository<Topic>
    {
        Task<Topic> GetOrCreateTopicAsync(string topicName);
        Task<Topic?> GetTopicByNameOrDefaultAsync(string topicName);
        Task<IEnumerable<Topic>> GetTopicRange(string[] topicNames);
        Task<bool> IsAllTopicExists(string[] topicNames);
    }
}
