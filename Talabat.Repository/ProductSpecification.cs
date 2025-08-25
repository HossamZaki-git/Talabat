using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Domain_Models;

namespace Talabat.Repository
{
    public class ProductSpecification : GenericSpecification<Product>
    {
        // For setting the expressions of the Where operator & the Include operator
        public ProductSpecification(Expression<Func<Product, bool>> whereExpression, bool useIncludes = true) : base(whereExpression)
        {
            if(useIncludes)
            {
                Includes.Add(P => P.brand);
                Includes.Add(P => P.category);
            }
        }

        // For setting the expressions of the Include operator
        // and optionally setting a sorting key and  brandID, categoryID for filtering
        public ProductSpecification(ProductsApiParams productsParams)
            : base(
                    (productsParams.brandID is not null || productsParams.typeId is not null || productsParams.Search is not null) ?
                    P => (!productsParams.brandID.HasValue || productsParams.brandID.Value == P.BrandID) &&
                    (!productsParams.typeId.HasValue || productsParams.typeId.Value == P.CategoryID) &&
                    (string.IsNullOrEmpty(productsParams.Search) || P.Name.ToLower().Contains(productsParams.Search))
                    : null
                  )
        {
            Includes.Add(P => P.brand);
            Includes.Add(P => P.category);

            productsParams.sort = productsParams.sort?.ToLower()?.Trim();

            switch (productsParams.sort)
            {
                case "priceasc":
                    SortingKeyAsc = P => P.Price;
                    break;
                case "pricedesc":
                    SortingKeyDesc = P => P.Price;
                    break;
                default:
                    SortingKeyAsc = P => P.Name;
                    break;
            }

            AddPagination(productsParams.PageSize, productsParams.PageIndex);
        }
    }
}
