using TrustNetwork.Application.Dtos;
using TrustNetwork.Domain.Common;

namespace TrustNetwork.Application.Services
{
    public interface IPeopleService
    {
        Task<Result<PersonReadDto>> CreatePersonAsync(PersonCreateDto personCreateDto);
        Task<Result<PersonReadDto>> GetPersonByLoginAsync(string login);
        Task<Result<IEnumerable<PersonReadDto>>> GetAllPeopleAsync();
    }
}