using Microsoft.AspNetCore.Mvc;
using TrustNetwork.Application.Dtos.Messages;
using TrustNetwork.Application.Services;

namespace TrustNetwork.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PathController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public PathController(IMessageService messageService) => _messageService = messageService;

        [HttpPost]
        public async Task<ActionResult<MessageReadDto>> SendMessage(MessageCreateDto createModel)
            => (await _messageService.SendMessage(createModel))
                    .Match<ActionResult>(
                        message => Ok(message),
                        error => error.HandleError());
    }
}
