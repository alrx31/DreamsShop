using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class Order
    {
        [Key]
        public required Guid Id { get; set; }

        public required decimal Cost { get; set; }

        public required Guid Consumer_Id { get; set; }

        public required Guid Transaction_Id { get; set; }
        
        

        public ConsumerUser ConsumerUser { get; set; }

        public ICollection<DreamInOrder> DreamInOrders { get; set; }

        public OrderTransaction OrderTransactions { get; set; }
    }
}