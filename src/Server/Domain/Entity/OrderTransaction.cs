using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class OrderTransaction
    {
        [Key]
        public required Guid Id { get; set; }

        public required OrderTransactionStatuses Status { get; set; }

        public required DateTime UpdatedAt { get; set; }

        public required  Guid Order_Id { get; set; }
        
        

        public ICollection<Order> Orders { get; set; }
    }
}