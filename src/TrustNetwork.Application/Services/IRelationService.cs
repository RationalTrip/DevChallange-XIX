using TrustNetwork.Application.Dtos.Relation;
using TrustNetwork.Domain.Common;

namespace TrustNetwork.Application.Services
{
    public interface IRelationService
    {
        Task<EmptyResult> CreateOrUpdateRelationsAsync(string senderLogin, RelationCreateDto createModel);
        Task<Result<RelationReadDto>> GetAllPersonRelationByLoginAsync(string senderLogin);
        Task<Result<RelationReadDto>> GetRelationByLoginsAsync(string senderLogin, string receiverLogin);
    }
}