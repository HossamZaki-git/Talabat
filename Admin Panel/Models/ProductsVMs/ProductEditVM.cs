using System.ComponentModel.DataAnnotations;

namespace Admin_Panel.Models.ProductsVMs
{
    public class ProductEditVM : Product_Edit_Create_BaseVM
    {
        public IFormFile? ImageFile { get; set; }
    }
}
