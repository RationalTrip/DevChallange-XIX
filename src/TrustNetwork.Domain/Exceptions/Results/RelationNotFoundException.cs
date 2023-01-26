namespace TrustNetwork.Domain.Exceptions.Results
{
    public class RelationNotFoundException : NotFoundException
    {
        public RelationNotFoundException(string senderLogin, string receiverLogin)
            : base($"Relation between sender \"{senderLogin}\" and \"{receiverLogin}\" not found") { }
    }
}
