using Microsoft.AspNetCore.Mvc;
using TrustNetwork.Application.Dtos;
using TrustNetwork.Application.Services;

namespace TrustNetwork.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly IPeopleService _peopleService;

        public PeopleController(IPeopleService peopleService) => _peopleService = peopleService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonReadDto>>> GetAll()
            => (await _peopleService.GetAllPeopleAsync())
                    .Match<ActionResult>(
                        persons => Ok(persons),
                        error => error.HandleError());

        [HttpGet("{login}", Name = nameof(GetPersonByLogin))]
        public async Task<ActionResult<PersonReadDto>> GetPersonByLogin(string login)
            => (await _peopleService.GetPersonByLoginAsync(login))
                    .Match<ActionResult>(
                        person => Ok(person),
                        error => error.HandleError());

        [HttpPost]
        public async Task<ActionResult<PersonReadDto>> CreatePerson(PersonCreateDto createModel)
            => (await _peopleService.CreatePersonAsync(createModel))
                    .Match<ActionResult>(
                        person => CreatedAtRoute(nameof(GetPersonByLogin), new { login = person.Id }, person),
                        error => error.HandleError());
    }
}
