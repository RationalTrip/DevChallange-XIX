using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TrustNetwork.Application.Dtos.Messages
{
    public class MessageCreateDto
    {
        [Required]
        public string Text { get; set; } = string.Empty;

        [Required]
        public string[] Topics { get; set; } = Array.Empty<string>();

        [Required]
        [JsonPropertyName("from_person_id")]
        public string SenderLogin { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("min_trust_level")]
        public int MinTrustLevel { get; set; }
    }
}
