using Stripe;
using System.ComponentModel.DataAnnotations;

namespace Admin_Panel.Models.ProductsVMs
{
    public class ProductCreationVM : Product_Edit_Create_BaseVM
    {
        [Required]
        public IFormFile ImageFile { get; set; }

        public static implicit operator Talabat.Core.Domain_Models.Product(ProductCreationVM value)
            => new Talabat.Core.Domain_Models.Product
            {
                Name = value.Name,
                Description = value.Description,
                BrandID = value.BrandID.Value,
                CategoryID = value.CategoryID.Value,
                Price = value.Price,
                PictureURL = value.PictureURL
            };
    }
}
