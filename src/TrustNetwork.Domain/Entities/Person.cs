using System.ComponentModel.DataAnnotations;

namespace TrustNetwork.Domain.Entities
{
    public class Person
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(32)]
        public string Login { get; set; } = string.Empty;
        public virtual List<Topic> Topics { get; set; } = new List<Topic>();

        public virtual List<Relation> PeopleSendTo { get; set; } = new List<Relation>();

        public virtual List<Relation> PeopleReceiveFrom { get; set; } = new List<Relation>();
    }
}
