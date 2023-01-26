using System.ComponentModel.DataAnnotations;

namespace TrustNetwork.Domain.Entities
{
    public class Relation
    {
        public int Id { get; set; }

        [Required]
        public virtual Person Sender { get; set; } = null!;
        public int SenderId { get; set; }

        [Required]
        public virtual Person Receiver { get; set; } = null!;
        public int ReceiverId { get; set; }

        public int TrustLevel { get; set; }
    }
}
