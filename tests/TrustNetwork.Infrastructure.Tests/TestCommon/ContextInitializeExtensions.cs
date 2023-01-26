using TrustNetwork.Domain.Entities;
using TrustNetwork.Infrastructure.Context;

namespace TrustNetwork.InfrastructureTests.TestCommon
{
    internal static class ContextInitializeExtensions
    {
        public static void UnitTestInitializeData(this TrustNetworkDbContext context)
        {
            //topic section
            var study = new Topic { Name = "study" };
            var fan = new Topic { Name = "fan" };
            var gryffindor = new Topic { Name = "gryffindor" };
            var power = new Topic { Name = "power" };
            var magic = new Topic { Name = "magic" };
            var animal = new Topic { Name = "animal" };
            var snakeKiller = new Topic { Name = "snakeKiller" };
            var teacher = new Topic { Name = "teacher" };

            context.Topics.AddRange(study, magic, fan, gryffindor, power);

            //person section
            var garry = new Person { Login = "Garry", Topics = { study, power, magic, gryffindor } };
            var snape = new Person { Login = "Snape", Topics = { magic, teacher } };
            var ron = new Person { Login = "Ron", Topics = { fan, gryffindor, magic } };
            var voldemort = new Person { Login = "Voldemort", Topics = { power, magic } };
            var hermione = new Person { Login = "Hermione", Topics = { study, magic, gryffindor } };
            var nelson = new Person { Login = "Nelson", Topics = { fan, magic, gryffindor, snakeKiller } };
            var dean = new Person { Login = "Dean", Topics = { magic, gryffindor } };
            var snake = new Person { Login = "Snake", Topics = { magic, animal } };
            var remus = new Person { Login = "Remus", Topics = { magic, power, gryffindor } };

            context.People.AddRange(garry, snape, ron, voldemort, hermione, nelson);

            //relation section

            //garry relation
            context.Relations.Add(new Relation { Sender = garry, Receiver = ron, TrustLevel = 10 });
            context.Relations.Add(new Relation { Sender = garry, Receiver = hermione, TrustLevel = 10 });
            context.Relations.Add(new Relation { Sender = garry, Receiver = snape, TrustLevel = 4 });
            context.Relations.Add(new Relation { Sender = garry, Receiver = voldemort, TrustLevel = 1 });

            //ron relation
            context.Relations.Add(new Relation { Sender = ron, Receiver = nelson, TrustLevel = 8 });
            context.Relations.Add(new Relation { Sender = ron, Receiver = dean, TrustLevel = 6 });

            //germion relation
            context.Relations.Add(new Relation { Sender = hermione, Receiver = snape, TrustLevel = 6 });

            //nelson relation
            context.Relations.Add(new Relation { Sender = nelson, Receiver = snape, TrustLevel = 1 });
            context.Relations.Add(new Relation { Sender = nelson, Receiver = hermione, TrustLevel = 5 });
            context.Relations.Add(new Relation { Sender = nelson, Receiver = ron, TrustLevel = 9 });
            context.Relations.Add(new Relation { Sender = nelson, Receiver = dean, TrustLevel = 10 });
            context.Relations.Add(new Relation { Sender = nelson, Receiver = remus, TrustLevel = 6 });

            //volendemord relation
            context.Relations.Add(new Relation { Sender = voldemort, Receiver = snake, TrustLevel = 9 });
        }
    }
}
