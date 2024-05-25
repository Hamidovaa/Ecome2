namespace Ecome2.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string ImageUrl { get; set; }
        public string ImageUrlBase {  get; set; }
        public decimal Total
        {
            get { return Quantity * Price; }
        }

        public CartItem()
        {
        }

        public CartItem(Products product)
        {
            ProductId = product.Id;
            ProductName = product.Title;
            Price = product.Price;
            ImageUrlBase = product.ImgUrlBase;
            ImageUrl = product.Images[0].ImgUrl;
           
        }
    }

}
