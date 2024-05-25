using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecome2.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int OrderNumber { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal Total { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string PostCode { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string PaymentMethod { get; set; }
        [Required]
        public string Shipping { get; set; }
        public string UserId { get; set; }
        public ProgramUser User { get; set; }

        public List<OrderItem> OrderItems { get; set; }= new List<OrderItem>();
    }
}



