using AutoMapper;
using Talabat.Core.Domain_Models;
using Talabat.Core.Domain_Models.Order_Module;
using Talabat.WebAPI.DTOs;

namespace Talabat.WebAPI.Utilities
{
    public class ImageURLResolver<S, D> : IValueResolver<S, D, string>
    {
        private readonly IConfiguration configuration;

        public ImageURLResolver(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string Resolve(S source, D destination, string destMember, ResolutionContext context)
        {
            if(typeof(S) == typeof(Product) && typeof(D) == typeof(ProductDTO))
            {
                Product Product_Source = source as Product;
                if (string.IsNullOrEmpty(Product_Source.PictureURL))
                    return "";
                return $"{configuration["WebAPIBaseURL"]}{Product_Source.PictureURL}";
            }

            if(typeof(S) == typeof(OrderItem) && typeof(D) == typeof(OrderItemDTO))
            {
                OrderItem OrderItem_Source = source as OrderItem;
                if (string.IsNullOrEmpty(OrderItem_Source.OrderedProduct.ImageURL))
                    return "";
                return $"{configuration["WebAPIBaseURL"]}{OrderItem_Source.OrderedProduct.ImageURL}";
            }

            if(typeof(S) == typeof(BasketItem))
            {
                BasketItem basketItem = source as BasketItem;
                if (string.IsNullOrEmpty(basketItem.PictureURL))
                    return "";
                return $"{configuration["WebAPIBaseURL"]}{basketItem.PictureURL}";
            }

            return "";
        }
    }

    #region Old Code
    /*
public class ProductDTOPictureResolver : IValueResolver<Product, ProductDTO, string>
{
private readonly IConfiguration configuration;

public ProductDTOPictureResolver(IConfiguration configuration)
{
this.configuration = configuration;
}
public string Resolve(Product source, ProductDTO destination, string destMember, ResolutionContext context)
{
if (string.IsNullOrEmpty(source.PictureURL))
    return "";
return $"{configuration["WebAPIBaseURL"]}{source.PictureURL}";
}
}
*/ 
    #endregion

}


