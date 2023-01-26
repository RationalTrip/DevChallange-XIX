using TrustNetwork.Application.Dtos.Messages;
using TrustNetwork.Domain.Common;

namespace TrustNetwork.Application.Services
{
    public interface IMessageService
    {
        Task<Result<MessageReadDto>> BroadcastMessage(MessageCreateDto messageCreateDto);
        Task<Result<MessageReadDto>> SendMessage(MessageCreateDto messageCreateDto);
    }
}
