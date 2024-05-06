using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Ecome2.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [ValidateNever] 
        public List<Products> Products { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
