using AutoMapper;
using TrustNetwork.Application.Dtos;
using TrustNetwork.Domain.Entities;

namespace TrustNetwork.Application.Profiles
{
    public class PersonsProfile : Profile
    {
        public PersonsProfile()
        {
            CreateMap<PersonCreateDto, Person>()
                .ForMember(person => person.Id, opt => opt.Ignore())
                .ForMember(person => person.Login, opt => opt.MapFrom(creator => creator.Id))
                .ForMember(person => person.Topics, opt => opt.Ignore());

            CreateMap<Person, PersonReadDto>()
                .ForMember(read => read.Id, opt => opt.MapFrom(person => person.Login))
                .ForMember(read => read.Topics,
                    opt => opt.MapFrom(person => person.Topics.Select(topic => topic.Name)));
        }
    }
}
