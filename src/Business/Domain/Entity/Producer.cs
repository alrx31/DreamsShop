using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class Producer
    {
        [Key]
        public required Guid Id { get; set; }

        [StringLength(50)]
        public required string Name { get; set; }

        [StringLength(1000)]
        public required string Description { get; set; }

        public decimal? Rating { get; set; }
        

        public ICollection<Dream> Dreams { get; set; }
        public ICollection<ProducerUser> ProducerUsers { get; set; }
        public ICollection<RatingsProducer> Raitings_Producers { get; set; }
    }
}