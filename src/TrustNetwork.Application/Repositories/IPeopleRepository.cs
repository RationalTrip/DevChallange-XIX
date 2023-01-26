using TrustNetwork.Domain.Entities;

namespace TrustNetwork.Application.Repositories
{
    public interface IPeopleRepository : IRepository<Person>
    {
        Task<Person> GetPersonByLoginAsync(string login);
        Task<Person?> GetPersonByLoginOrDefaultAsync(string login);

        Task FillPersonWithExistingOrNewTopicsAsync(Person person, string[] topicsName);
    }
}
