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
    }
}
