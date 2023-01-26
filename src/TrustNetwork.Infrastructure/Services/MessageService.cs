using TrustNetwork.Application.Dtos.Messages;
using TrustNetwork.Application.Repositories;
using TrustNetwork.Application.Services;
using TrustNetwork.Domain.Common;
using TrustNetwork.Domain.Entities;
using TrustNetwork.Domain.Exceptions.Results;

namespace TrustNetwork.Infrastructure.Services
{
    public class MessageService : IMessageService
    {
        private readonly IPeopleRepository _peopleRepo;
        private readonly ITopicsRepository _topicRepo;

        public MessageService(IPeopleRepository peopleRepo, ITopicsRepository topicRepo)
        {
            _peopleRepo = peopleRepo;
            _topicRepo = topicRepo;
        }
        public async Task<Result<MessageReadDto>> BroadcastMessage(MessageCreateDto messageCreateDto)
            => await HandleMessage(messageCreateDto, MessageType.Broadcast);

        public async Task<Result<MessageReadDto>> SendMessage(MessageCreateDto messageCreateDto)
            => await HandleMessage(messageCreateDto, MessageType.Singlecast);

        private async Task<Result<MessageReadDto>> HandleMessage(MessageCreateDto messageCreateDto, MessageType messageType)
        {
            var sender = await _peopleRepo.GetPersonByLoginOrDefaultAsync(messageCreateDto.SenderLogin);
            if (sender is null)
                return new(new PersonLoginNotFoundException(messageCreateDto.SenderLogin));

            if (!(await _topicRepo.IsAllTopicExists(messageCreateDto.Topics)))
                return new MessageReadDto { SenderLogin = sender.Login };

            var messageTopics = await _topicRepo.GetTopicRange(messageCreateDto.Topics);

            var minTrustLevel = messageCreateDto.MinTrustLevel;

            var messageManager = new MessageHandleManager(sender, messageTopics, minTrustLevel);

            var sendHistory =
                messageType switch
                {
                    MessageType.Broadcast => messageManager.BrodcastMessage(),
                    MessageType.Singlecast => messageManager.SendMessage(),
                    _ => throw new NotImplementedException($"Message type {messageType} is unsupported!")
                };

            return new MessageReadDto
            {
                SenderLogin = sender.Login,
                MessageReceived = sendHistory.Select(person => person.Login)
            };
        }

        private enum MessageType : byte
        {
            Singlecast,
            Broadcast
        }
        private class MessageHandleManager
        {
            private IEnumerable<Topic> _messageTopics;
            private int _minTrustLevel;
            private HashSet<int> _alreadyInMessageHistory;
            private Person _sender;

            public MessageHandleManager(Person sender, IEnumerable<Topic> messageTopics, int minTrustLevel)
            {
                _messageTopics = messageTopics;
                _minTrustLevel = minTrustLevel;
                _alreadyInMessageHistory = new HashSet<int>();
                _sender = sender;
            }

            public IEnumerable<Person> BrodcastMessage()
            {
                var receivers = GetPersonToSendWithRequiredTopics(_sender).ToList();
                receivers.ForEach(rec => _alreadyInMessageHistory.Add(rec.Id));

                var messagePath = new List<Person>(receivers);

                for (int messagePathPosition = 0; messagePathPosition < messagePath.Count; messagePathPosition++)
                {
                    var currentSender = messagePath[messagePathPosition];

                    receivers = GetPersonToSendWithRequiredTopics(currentSender).ToList();
                    receivers.ForEach(rec => _alreadyInMessageHistory.Add(rec.Id));

                    messagePath.AddRange(receivers);
                }

                return messagePath;
            }
            public IEnumerable<Person> SendMessage()
            {
                var messagePath = new List<MessageSendChain>();
                int messagePathPosition = -1;
                var currentSender = _sender;

                var iterationResult = MessageSendIteration();
                if (iterationResult is not null)
                    return iterationResult;

                for (messagePathPosition = 0; messagePathPosition < messagePath.Count; messagePathPosition++)
                {
                    currentSender = messagePath[messagePathPosition].Receiver;

                    _alreadyInMessageHistory.Add(currentSender.Id);

                    iterationResult = MessageSendIteration();
                    if (iterationResult is not null)
                        return iterationResult;
                }

                return Array.Empty<Person>();

                IEnumerable<Person>? MessageSendIteration()
                {
                    var receivers = GetPersonToSend(currentSender!);

                    var filteredReceivers = FilterReceiversByTopics(receivers);
                    if (filteredReceivers.Any())
                        return MakeMessageSendChain(messagePath!, 
                            new MessageSendChain(filteredReceivers.First(), messagePathPosition));

                    messagePath!.AddRange(receivers.Select(rec => new MessageSendChain(rec, messagePathPosition)));

                    return null;
                }
            }

            private IEnumerable<Person> GetPersonToSendWithRequiredTopics(Person sender)
                => FilterReceiversByTopics(GetPersonToSend(sender));

            private IEnumerable<Person> GetPersonToSend(Person sender)
                => from relation in sender.PeopleSendTo
                   where relation.TrustLevel >= _minTrustLevel
                   let receiver = relation.Receiver
                   where !_alreadyInMessageHistory.Contains(receiver.Id)
                   select receiver;

            private IEnumerable<Person> FilterReceiversByTopics(IEnumerable<Person> receivers)
                => receivers.Where(rec => _messageTopics.All(topic => rec.Topics.Contains(topic)));

            private IEnumerable<Person> MakeMessageSendChain(List<MessageSendChain> messageChain, MessageSendChain lastReceiver)
                => MakeMessageSendChainFromLast(messageChain, lastReceiver)
                        .Reverse()
                        .ToList();

            private IEnumerable<Person> MakeMessageSendChainFromLast(List<MessageSendChain> messageChain, MessageSendChain lastReceiver)
            {
                var receiver = lastReceiver;

                while (receiver.SenderIndex >= 0)
                {
                    yield return receiver.Receiver;

                    receiver = messageChain[receiver.SenderIndex];
                }

                yield return receiver.Receiver;
            }

            private record struct MessageSendChain(Person Receiver, int SenderIndex);
        }
    }
}
