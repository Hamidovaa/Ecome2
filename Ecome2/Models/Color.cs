using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Ecome2.Models
{
    public class Color
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        public bool IsActive { get; set; } = true;
        [ValidateNever]
        public List<ProductColor> ProductColors { get; set; }
    }
}
