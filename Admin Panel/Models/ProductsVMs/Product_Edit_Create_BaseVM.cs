using System.ComponentModel.DataAnnotations;

namespace Admin_Panel.Models.ProductsVMs
{
    public class Product_Edit_Create_BaseVM : ProductVM
    {
        [Required]
        public string Description { get; set; }
        [Required]
        public int? CategoryID { get; set; }
        [Required]
        public int? BrandID { get; set; }
    }
}
