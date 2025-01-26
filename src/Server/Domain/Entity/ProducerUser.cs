using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class ProducerUser
    {
        [Key]
        public required Guid Id { get; set; }

        [StringLength(50)]
        public required string Name { get; set; }

        [StringLength(50)]
        public required string Email { get; set; }

        [StringLength(50)]
        public required string Password { get; set; }

        public required Roles Role { get; set; }

        public required Guid ProducerId { get; set; }
        

        public Producer Producer { get; set; }
    }
}