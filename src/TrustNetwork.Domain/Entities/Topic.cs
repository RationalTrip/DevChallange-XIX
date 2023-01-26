using System.ComponentModel.DataAnnotations;

namespace TrustNetwork.Domain.Entities
{
    public class Topic
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public virtual List<Person> Persons { get; set; } = new List<Person>();
    }
}
