using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class Dream
    {
        [Key]
        public required Guid Id { get; set; }

        [StringLength(50)]
        public required string Title { get; set; }

        [StringLength(1000)]
        public required string Desctiption { get; set; }

        public required Guid ImageMediaId { get; set; }

        public required Guid PreviewMediaId { get; set; }

        public required Guid ProducerId { get; set; }

        public decimal? Rating { get; set; }
        

        public Media ImageMedia { get; set; }
        public Media PreviewMedia { get; set; }
        public Producer Producer { get; set; }
        public ICollection<DreamInCategory> DreamInCategories { get; set; }
        public ICollection<DreamInOrder> DreamInOrders { get; set; }
        public ICollection<RatingsDreams> Raitings_Dreamses { get; set; }
    }
}