using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TrustNetwork.Application.Dtos.Messages;
using TrustNetwork.Application.Repositories;
using TrustNetwork.Application.Services;
using TrustNetwork.Infrastructure.Context;
using TrustNetwork.Infrastructure.Repositories;
using TrustNetwork.InfrastructureTests.TestCommon;
using Xunit;

namespace TrustNetwork.Infrastructure.Services.Tests
{
    public class MessageServiceTests : IDisposable
    {
        private TrustNetworkDbContext _context = null!;
        private ITopicsRepository _topicRepo = null!;
        private IPeopleRepository _peopleRepo = null!;

        public MessageServiceTests()
        {
            var options = new DbContextOptionsBuilder().UseInMemoryDatabase("messageServiceTest");
            _context = new TrustNetworkDbContext(options.Options);

            _context.UnitTestInitializeData();

            _context.SaveChanges();

            _topicRepo = new TopicsRepository(_context);
            _peopleRepo = new PeopleRepository(_context, _topicRepo);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }

        [Theory]
        [MemberData(nameof(GetValidBroadcastValues))]
        public async Task BroadcastMessageTest_VaidData_MessageReadDto(MessageCreateDto input,
            MessageReadDto expectedResult)
        {
            //Arrange
            IMessageService messageService = new MessageService(_peopleRepo, _topicRepo);

            //Act
            var actualResult = await messageService.BroadcastMessage(input);

            //Assert
            actualResult.IsSuccess.Should().BeTrue();

            var actualValue = actualResult.Value;

            actualValue.SenderLogin.Should().Be(expectedResult.SenderLogin);

            actualValue.MessageReceived.Should().BeEquivalentTo(expectedResult.MessageReceived);
        }

        [Theory]
        [MemberData(nameof(GetValidSinglecastValues))]
        public async Task SendMessageTest_VaidData_MessageReadDto(MessageCreateDto input,
            MessageReadDto expectedResult)
        {
            //Arrange
            IMessageService messageService = new MessageService(_peopleRepo, _topicRepo);

            //Act
            var actualResult = await messageService.SendMessage(input);

            //Assert
            actualResult.IsSuccess.Should().BeTrue();

            var actualValue = actualResult.Value;

            actualValue.SenderLogin.Should().Be(expectedResult.SenderLogin);

            actualValue.MessageReceived.Should().BeEquivalentTo(expectedResult.MessageReceived);
        }

        public static IEnumerable<object[]> GetValidSinglecastValues()
        {
            yield return new object[]
            {
                new MessageCreateDto
                {
                    Text="Who kill the snake?",
                    SenderLogin="Garry",
                    Topics= new string[]{ "snakeKiller"},
                    MinTrustLevel=4
                },
                new MessageReadDto
                {
                    SenderLogin="Garry",
                    MessageReceived=new string[] { "Ron", "Nelson" }
                }
            };

            yield return new object[]
            {
                new MessageCreateDto
                {
                    Text="Animal?",
                    SenderLogin="Garry",
                    Topics= new string[]{"magic", "animal"},
                    MinTrustLevel=1
                },
                new MessageReadDto
                {
                    SenderLogin="Garry",
                    MessageReceived=new string[] { "Voldemort", "Snake" }
                }
            };

            yield return new object[]
            {
                new MessageCreateDto
                {
                    Text="Who is strong",
                    SenderLogin="Garry",
                    Topics= new string[]{"power"},
                    MinTrustLevel=1
                },
                new MessageReadDto
                {
                    SenderLogin="Garry",
                    MessageReceived=new string[] { "Voldemort"}
                }
            };

            yield return new object[]
            {
                new MessageCreateDto
                {
                    Text="Who is strong in gryffindor",
                    SenderLogin="Garry",
                    Topics= new string[]{"power", "gryffindor"},
                    MinTrustLevel=1
                },
                new MessageReadDto
                {
                    SenderLogin="Garry",
                    MessageReceived=new string[] { "Ron", "Nelson", "Remus" }
                }
            };

            yield return new object[]
            {
                new MessageCreateDto
                {
                    Text="Who is strong in gryffindor",
                    SenderLogin="Garry",
                    Topics= new string[]{ "teacher"},
                    MinTrustLevel=5
                },
                new MessageReadDto
                {
                    SenderLogin="Garry",
                    MessageReceived=new string[] { "Hermione", "Snape" }
                }
            };

            yield return new object[]
            {
                new MessageCreateDto
                {
                    Text="Who is strong in gryffindor",
                    SenderLogin="Garry",
                    Topics= new string[]{ "teacher"},
                    MinTrustLevel=4
                },
                new MessageReadDto
                {
                    SenderLogin="Garry",
                    MessageReceived=new string[] { "Snape" }
                }
            };
        }

        public static IEnumerable<object[]> GetValidBroadcastValues()
        {
            yield return new object[]
            {
                new MessageCreateDto
                {
                    Text="Who kill the snake?",
                    SenderLogin="Garry",
                    Topics= new string[]{"magic"},
                    MinTrustLevel=10
                },
                new MessageReadDto
                {
                    SenderLogin="Garry",
                    MessageReceived=new string[] { "Hermione", "Ron" }
                }
            };

            yield return new object[]
            {
                new MessageCreateDto
                {
                    Text="Hello World",
                    SenderLogin="Garry",
                    Topics= new string[]{"magic"},
                    MinTrustLevel=1
                },
                new MessageReadDto
                {
                    SenderLogin="Garry",
                    MessageReceived=new string[] { "Remus", "Snake", "Dean", "Nelson", "Hermione",
                        "Voldemort", "Ron", "Snape"}
                }
            };

            yield return new object[]
            {
                new MessageCreateDto
                {
                    Text="Hello World",
                    SenderLogin="Garry",
                    Topics= new string[]{"magic"},
                    MinTrustLevel=5
                },
                new MessageReadDto
                {
                    SenderLogin="Garry",
                    MessageReceived=new string[] { "Remus", "Dean", "Nelson", "Hermione", "Ron", "Snape"}
                }
            };

            yield return new object[]
            {
                new MessageCreateDto
                {
                    Text="Hello World",
                    SenderLogin="Garry",
                    Topics= new string[]{"gryffindor"},
                    MinTrustLevel=8
                },
                new MessageReadDto
                {
                    SenderLogin="Garry",
                    MessageReceived=new string[] { "Dean", "Nelson", "Hermione", "Ron"}
                }
            };

            yield return new object[]
            {
                new MessageCreateDto
                {
                    Text="Hello World",
                    SenderLogin="Garry",
                    Topics= new string[]{"gryffindor"},
                    MinTrustLevel=5
                },
                new MessageReadDto
                {
                    SenderLogin="Garry",
                    MessageReceived=new string[] { "Dean", "Nelson", "Hermione", "Ron", "Remus" }
                }
            };
        }
    }
}