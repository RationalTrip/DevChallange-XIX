using TrustNetwork.Application.Dtos.Relation;
using TrustNetwork.Application.Repositories;
using TrustNetwork.Application.Services;
using TrustNetwork.Domain.Common;
using TrustNetwork.Domain.Exceptions.Results;

namespace TrustNetwork.Infrastructure.Services
{
    public class RelationService : IRelationService
    {
        private readonly IRelationsRepository _relationRepo;
        private readonly IPeopleRepository _peopleRepo;

        public RelationService(IRelationsRepository relationRepo, IPeopleRepository peopleRepo)
        {
            _relationRepo = relationRepo;
            _peopleRepo = peopleRepo;
        }

        public async Task<Result<RelationReadDto>> GetRelationByLoginsAsync(string senderLogin, string receiverLogin)
        {
            var sender = await _peopleRepo.GetPersonByLoginOrDefaultAsync(senderLogin);
            if (sender is null)
                return new(new PersonLoginNotFoundException(senderLogin));

            var receiver = await _peopleRepo.GetPersonByLoginOrDefaultAsync(receiverLogin);
            if (receiver is null)
                return new(new PersonLoginNotFoundException(receiverLogin));

            var relation = await _relationRepo.GetRelationByPersonsOrDefaultAsync(sender.Id, receiver.Id);
            if (relation is null)
                return new(new RelationNotFoundException(sender.Login, receiver.Login));

            return new RelationReadDto
            {
                { receiver.Login, relation.TrustLevel }
            };
        }

        public async Task<Result<RelationReadDto>> GetAllPersonRelationByLoginAsync(string senderLogin)
        {
            var sender = await _peopleRepo.GetPersonByLoginOrDefaultAsync(senderLogin);
            if (sender is null)
                return new(new PersonLoginNotFoundException(senderLogin));

            return new RelationReadDto(
                sender.PeopleSendTo
                    .Select(relation => (relation.Receiver.Login, relation.TrustLevel)));
        }

        public async Task<EmptyResult> CreateOrUpdateRelationsAsync(string senderLogin, RelationCreateDto createModel)
        {
            var sender = await _peopleRepo.GetPersonByLoginOrDefaultAsync(senderLogin);
            if (sender is null)
                return new(new PersonLoginNotFoundException(senderLogin));

            foreach (var receiverLogin in createModel.GetPeopleLogins())
            {
                if (senderLogin == receiverLogin)
                    return new(new LoopRelationForbiddenException(senderLogin));

                if (!(await _peopleRepo.IsExistsAsync(person => person.Login == receiverLogin)))
                    return new(new PersonLoginNotFoundException(receiverLogin));
            }

            foreach (var (receiverLogin, trustLevel) in createModel.EnumerateRelation())
            {
                var receiver = await _peopleRepo.GetPersonByLoginAsync(receiverLogin);

                await _relationRepo.CreateOrUpdateRelationsAsync(sender, receiver, trustLevel);
            }

            await _relationRepo.SaveChangesAsync();

            return EmptyResult.SuccessResult;
        }
    }
}
