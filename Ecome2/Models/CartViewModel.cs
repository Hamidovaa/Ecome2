namespace Ecome2.Models
{
    public class CartViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public decimal GrandTotal { get; set; }
        public CartViewModel()
        {
            CartItems = new List<CartItem>();
        }
    }
}
