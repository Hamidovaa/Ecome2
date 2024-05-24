using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Ecome2.Models
{
    public class Size
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
        [ValidateNever]
        public List<ProductSize> ProductSizes { get; set; }
    }
}
