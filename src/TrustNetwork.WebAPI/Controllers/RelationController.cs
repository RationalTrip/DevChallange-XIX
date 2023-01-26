using Microsoft.AspNetCore.Mvc;
using TrustNetwork.Application.Dtos.Relation;
using TrustNetwork.Application.Services;

namespace TrustNetwork.WebAPI.Controllers
{
    [Route("api/people/{senderLogin}/trust_connections")]
    [ApiController]
    public class RelationController : ControllerBase
    {
        private readonly IRelationService _relationService;

        public RelationController(IRelationService relationService) => _relationService = relationService;

        [HttpGet("{receiverLogin}")]
        public async Task<ActionResult<RelationReadDto>> GetRelstionForPersons(string senderLogin, string receiverLogin)
            => (await _relationService.GetRelationByLoginsAsync(senderLogin, receiverLogin))
                    .Match<ActionResult>(
                        relation => Ok(relation),
                        error => error.HandleError());

        [HttpGet]
        public async Task<ActionResult<RelationReadDto>> GetRelstionForPersons(string senderLogin)
            => (await _relationService.GetAllPersonRelationByLoginAsync(senderLogin))
                    .Match<ActionResult>(
                        relation => Ok(relation),
                        error => error.HandleError());

        [HttpPost]
        public async Task<ActionResult> CreateRelation(string senderLogin, RelationCreateDto createModel)
            => (await _relationService.CreateOrUpdateRelationsAsync(senderLogin, createModel))
                    .Match<ActionResult>(
                        () => Ok(),
                        error => error.HandleError());
    }
}
