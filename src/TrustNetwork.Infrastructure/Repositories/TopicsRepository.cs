using Microsoft.EntityFrameworkCore;
using TrustNetwork.Application.Repositories;
using TrustNetwork.Domain.Entities;
using TrustNetwork.Infrastructure.Context;

namespace TrustNetwork.Infrastructure.Repositories
{
    public class TopicsRepository : Repository<Topic>, IRepository<Topic>, ITopicsRepository
    {
        private readonly TrustNetworkDbContext _context;
        public TopicsRepository(TrustNetworkDbContext context) : base(context) => _context = context;

        public async Task<Topic> GetOrCreateTopicAsync(string topicName)
        {
            var topic = await GetTopicByNameOrDefaultAsync(topicName);

            if (topic is not null)
                return topic;

            topic = new Topic { Name = topicName };

            Add(topic);

            return topic;
        }

        public async Task<Topic?> GetTopicByNameOrDefaultAsync(string topicName)
            => await _context.Topics.SingleOrDefaultAsync(topic => topic.Name == topicName);

        public async Task<Topic> GetTopicByName(string topicName)
            => await _context.Topics.SingleAsync(topic => topic.Name == topicName);

        public async Task<IEnumerable<Topic>> GetTopicRange(string[] topicNames)
        {
            var topics = new List<Topic>(topicNames.Length);

            foreach (var name in topicNames)
            {
                topics.Add(await GetTopicByName(name));
            }
            return topics;
        }

        public async Task<bool> IsAllTopicExists(string[] topicNames)
        {
            
            foreach (var name in topicNames)
            {
                if (!await IsExistsAsync(topic => topic.Name == name))
                    return false;
            }

            return true;
        }
    }
}
