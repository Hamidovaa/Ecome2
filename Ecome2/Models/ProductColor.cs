using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecome2.Models
{
    public class ProductColor
    {
        [Key]
        public int ProductColorId { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Products Product { get; set; }

        public int ColorId { get; set; }
        [ForeignKey("ColorId")]
        public Color Color { get; set; }
    }
}
