using System.ComponentModel.DataAnnotations;

namespace TrustNetwork.Application.Dtos
{
    public class PersonCreateDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(32)]
        public string Id { get; set; } = string.Empty;

        [Required]
        public string[] Topics { get; set; } = Array.Empty<string>();
    }
}
