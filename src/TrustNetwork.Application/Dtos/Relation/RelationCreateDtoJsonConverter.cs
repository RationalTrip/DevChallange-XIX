using System.Text.Json;
using System.Text.Json.Serialization;

namespace TrustNetwork.Application.Dtos.Relation
{
    public class RelationCreateDtoJsonConverter : JsonConverter<RelationCreateDto>
    {
        public override RelationCreateDto? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dictionary = JsonSerializer.Deserialize<Dictionary<string, int>>(ref reader, options);
            if (dictionary is null)
                return null;

            return new(dictionary);
        }

        public override void Write(Utf8JsonWriter writer, RelationCreateDto value, JsonSerializerOptions options)
            => JsonSerializer.Serialize<Dictionary<string, int>>(writer, value.Relations, options);
    }
}
