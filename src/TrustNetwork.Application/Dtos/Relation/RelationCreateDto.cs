using System.Text.Json.Serialization;

namespace TrustNetwork.Application.Dtos.Relation
{
    [JsonConverter(typeof(RelationCreateDtoJsonConverter))]
    public class RelationCreateDto
    {
        public RelationCreateDto() => Relations = new Dictionary<string, int>();
        internal RelationCreateDto(Dictionary<string, int> relations) => Relations = relations;

        internal Dictionary<string, int> Relations { get; set; }
        public IEnumerable<string> GetPeopleLogins() => Relations.Keys;
        public IEnumerable<(string receiverLogin, int trustLevel)> EnumerateRelation()
            => Relations.AsEnumerable()
                .Select(relation => (relation.Key, relation.Value));
    }
}
