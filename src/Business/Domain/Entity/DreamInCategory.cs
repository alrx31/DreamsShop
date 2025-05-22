using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class DreamInCategory
    {
        [Key]
        public required Guid Id { get; set; }

        public required Guid DreamId { get; set; }

        public required Guid CategoryId { get; set; }
        

        public Dream Dream { get; set; }

        public Category Category { get; set; }
    }
}