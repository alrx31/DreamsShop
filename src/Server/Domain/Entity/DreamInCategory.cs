using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class DreamInCategory
    {
        [Key]
        public required Guid Id { get; set; }

        public required Guid Dream_Id { get; set; }

        public required Guid Category_Id { get; set; }
        
        

        public Dream Dream { get; set; }

        public Category Category { get; set; }
    }
}