namespace TrustNetwork.Application.Dtos.Relation
{
    public class RelationReadDto : Dictionary<string, int>
    {
        public RelationReadDto() { }
        public RelationReadDto(IEnumerable<(string login, int trustLevel)> relation)
        {
            foreach (var (login, trustLevel) in relation)
            {
                this[login] = trustLevel;
            }
        }
    }
}
