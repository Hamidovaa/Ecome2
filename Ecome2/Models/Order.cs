using System.ComponentModel.DataAnnotations.Schema;

namespace Ecome2.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        [ForeignKey("ProductId")]
        public Products Products { get; set; }
        [ForeignKey("UserId")]
        public Users Users { get; set; } //programuser
    }
}



