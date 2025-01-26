using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class Category
    {
        [Key]
        public required Guid Id { get; set; }

        [StringLength(50)]
        public required string Title { get; set; }
        
        
        public ICollection<DreamInCategory> DreamInCategories { get; set; }
    }
}