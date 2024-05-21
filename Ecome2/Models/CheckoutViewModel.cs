namespace Ecome2.Models
{
    public class CheckoutViewModel
    {
        public Order Order { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
