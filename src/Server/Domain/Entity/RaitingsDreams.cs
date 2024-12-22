using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class RatingsDreams
    {
        [Key]
        public required Guid Id { get; set; }

        public required Guid Dream_Id { get; set; }

        public required Guid Consumer_Id { get; set; }

        public required int Value { get; set; }

        public required DateTime CreatedAt { get; set; }
        
        

        public ConsumerUser ConsumerUser { get; set; }

        public Dream Dream { get; set; }
    }
}