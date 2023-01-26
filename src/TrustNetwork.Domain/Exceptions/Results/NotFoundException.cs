namespace TrustNetwork.Domain.Exceptions.Results
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }
}
