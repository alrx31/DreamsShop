using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class Dream
    {
        [Key]
        public required Guid DreamId { get; init; }

        [StringLength(100)]
        public required string Title { get; set; }
        [StringLength(2000)]
        public required string Description { get; set; }
        public Guid? ProducerId { get; set; }
        public decimal? Rating { get; set; }
        public required string ImageFileName { get; set; }

        public IEnumerable<DreamCategory>? DreamCategories { get; set; }
        public IEnumerable<OrderDream>? OrderDreams { get; set; }
    }
}