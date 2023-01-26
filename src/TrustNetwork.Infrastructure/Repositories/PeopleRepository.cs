using Microsoft.EntityFrameworkCore;
using TrustNetwork.Application.Repositories;
using TrustNetwork.Domain.Entities;
using TrustNetwork.Infrastructure.Context;

namespace TrustNetwork.Infrastructure.Repositories
{
    public class PeopleRepository : Repository<Person>, IRepository<Person>, IPeopleRepository
    {
        private readonly TrustNetworkDbContext _context;
        private readonly ITopicsRepository _topicsRepo;

        public PeopleRepository(TrustNetworkDbContext context, ITopicsRepository topicsRepo) : base(context)
        {
            _context = context;
            _topicsRepo = topicsRepo;
        }

        public async Task<Person> GetPersonByLoginAsync(string login)
            => await _context.People.SingleAsync(person => person.Login == login);

        public async Task<Person?> GetPersonByLoginOrDefaultAsync(string login)
            => await _context.People.SingleOrDefaultAsync(person => person.Login == login);

        public async Task FillPersonWithExistingOrNewTopicsAsync(Person person, string[] topicsName)
        {
            foreach (var topic in topicsName)
            {
                person.Topics.Add(await _topicsRepo.GetOrCreateTopicAsync(topic));
            }
        }
    }
}
