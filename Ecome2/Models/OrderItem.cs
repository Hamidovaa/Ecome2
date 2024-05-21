using System.ComponentModel.DataAnnotations.Schema;

namespace Ecome2.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public int ProductId { get; set; }
        public Products Product { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
