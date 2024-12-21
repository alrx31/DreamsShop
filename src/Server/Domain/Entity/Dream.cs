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

        public required Guid Image_Media_Id { get; set; }

        public required Guid Preview_Media_Id { get; set; }

        public required Guid Producer_Id { get; set; }

        public decimal? Raiting { get; set; }
        
        

        public Media Image_Media { get; set; }
        
        public Media Preview_Media { get; set; }
        
        public Producer Producer { get; set; }

        public ICollection<DreamInCategory> DreamInCategories { get; set; }

        public ICollection<DreamInOrder> DreamInOrders { get; set; }
        
        public ICollection<RatingsDreams> Raitings_Dreamses { get; set; }
    }
}