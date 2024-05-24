using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecome2.Models
{
    public class ProductSize
    {
        [Key]
        public int ProductSizeId { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Products Product { get; set; }

        public int SizeId { get; set; }
        [ForeignKey("SizeId")]
        public Size Size { get; set; }
    }
}
