using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class ConsumerUser
    {
        [Key]
        public required Guid Id { get; set; }

        [StringLength(50)]
        public required string Email { get; set; }

        [StringLength(50)]
        public required string Password { get; set; }

        [StringLength(50)]
        public required string Name { get; set; }

        public required Roles Role { get; set; }
        

        public ICollection<Order> Orders { get; set; }
        public ICollection<RatingsDreams> RaitingsDreamses { get; set; }
        public ICollection<RatingsProducer> RaitingsProducers { get; set; }
    }
}