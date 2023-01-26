namespace TrustNetwork.Domain.Exceptions.Results
{
    public class PersonLoginNotFoundException : NotFoundException
    {
        public PersonLoginNotFoundException(string login)
            : base($"Person with login \"{login}\" not exists") { }
    }
}
