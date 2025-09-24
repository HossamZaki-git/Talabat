namespace Admin_Panel.Models.ProductsVMs
{
    public class ProductVM
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string? PictureURL { get; set; }
        public string? BrandName { get; set; }
        public string? CategoryName { get; set; }
    }
}
