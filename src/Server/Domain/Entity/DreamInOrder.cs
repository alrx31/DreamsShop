using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class DreamInOrder
    {
        [Key]
        public required Guid Id { get; set; }

        public required Guid Dream_Id { get; set; }

        public required Guid Order_Id { get; set; }

        public required DateTime AddDate { get; set; }

        public float? Raiting { get; set; }

        
        
        public Order Order { get; set; }

        public Dream Dream { get; set; }
    }
}