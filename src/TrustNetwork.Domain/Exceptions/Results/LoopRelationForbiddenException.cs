using System.ComponentModel.DataAnnotations;

namespace TrustNetwork.Domain.Exceptions.Results
{
    public class LoopRelationForbiddenException : ValidationException
    {
        public LoopRelationForbiddenException(string loopRelationLogin)
            : base($"Loop relation is forbidden. Was send 2 same logins {loopRelationLogin}") { }
    }
}
