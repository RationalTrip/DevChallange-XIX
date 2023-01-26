using AutoMapper;
using TrustNetwork.Application.Dtos;
using TrustNetwork.Application.Repositories;
using TrustNetwork.Application.Services;
using TrustNetwork.Domain.Common;
using TrustNetwork.Domain.Entities;
using TrustNetwork.Domain.Exceptions.Results;

namespace TrustNetwork.Infrastructure.Services
{
    public class PeopleService : IPeopleService
    {
        private readonly IMapper _mapper;
        private readonly IPeopleRepository _peopleRepo;

        public PeopleService(IMapper mapper, IPeopleRepository peopleRepo)
        {
            _mapper = mapper;
            _peopleRepo = peopleRepo;
        }

        public async Task<Result<PersonReadDto>> CreatePersonAsync(PersonCreateDto personCreateDto)
        {
            var personLogin = personCreateDto.Id;

            if (await _peopleRepo.IsExistsAsync(person => person.Login == personLogin))
                return new(new PersonWithSuchLoginAlreadyExistException(personLogin));


            var person = _mapper.Map<Person>(personCreateDto);
            var topicNames = personCreateDto.Topics;

            _peopleRepo.Add(person);
            await _peopleRepo.FillPersonWithExistingOrNewTopicsAsync(person, topicNames);

            await _peopleRepo.SaveChangesAsync();

            return _mapper.Map<PersonReadDto>(person);
        }

        public async Task<Result<PersonReadDto>> GetPersonByLoginAsync(string login)
        {
            var person = await _peopleRepo.GetPersonByLoginOrDefaultAsync(login);

            if (person is not null)
                return _mapper.Map<PersonReadDto>(person);

            return new(new PersonLoginNotFoundException(login));
        }

        public async Task<Result<IEnumerable<PersonReadDto>>> GetAllPeopleAsync()
        {
            var people = await _peopleRepo.GetAllAsync();

            return new(_mapper.Map<IEnumerable<PersonReadDto>>(people));
        }
    }
}
