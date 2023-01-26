using System.ComponentModel.DataAnnotations;

namespace TrustNetwork.Domain.Exceptions.Results
{
    public class PersonWithSuchLoginAlreadyExistException : ValidationException
    {
        public PersonWithSuchLoginAlreadyExistException(string login)
            : base($"People with login \"{login}\" already exists") { }
    }
}
