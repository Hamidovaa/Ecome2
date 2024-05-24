using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecome2.Models
{
    public class Products
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }
        public string? ImgUrlBase { get; set; }
        [NotMapped]
        [Required]
        public IFormFile file { get; set; }
        public bool IsActive { get; set; } = true;
        [ValidateNever]
        public List<OrderItem> OrderItems { get; set; }
        [ValidateNever]
        public List<Images> Images { get; set; }
        [NotMapped]
        [ValidateNever]
        public List<IFormFile> ImagesFiles { get; set; }
        [Required]
        public int StockQuantity { get; set; }
        [ValidateNever]
        public List<ProductColor> ProductColors { get; set; }
        [ValidateNever]
        public List<ProductSize> ProductSizes { get; set; }
        [NotMapped]
        [ValidateNever]
        public List<int> ColorsId { get; set; }
    }
}
