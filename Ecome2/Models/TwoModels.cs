namespace Ecome2.Models
{
    public class TwoModels
    {
        public List<Slider> sliders { get; set; }
        public List<Category> categories { get; set; }
        public List<Products> products { get; set; }
        public CartViewModel CartModel { get; set; }
        public List<Color> colors { get; set; }
        public List<Size> sizes { get; set; }
        public Order Order { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public string ViewMode { get; set; }


        public List<int> SelectedColors { get; set; }
        public List<int> SelectedSizes { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
    }
}
