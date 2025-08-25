using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Talabat.Core;
using Talabat.Core.Domain_Models;
using Talabat.Core.GenericRepository;
using Talabat.Repository;
using Talabat.WebAPI.DTOs;
using Talabat.WebAPI.Errors;
using Talabat.WebAPI.Utilities;

namespace Talabat.WebAPI.Controllers
{
    // The BaseAPIController is a (UDT) which has the [Route("...")] & the [ApiController] attributes
    // It also inherits from ControllerBase
    /* We gathered all these in the BaseAPIController and made any api controller inherits from it */
    
    public class ProductsController : BaseAPIController
    {
        private readonly IUnitOfWork unitOf;
        private readonly IMapper mapper;

        public ProductsController(
            IUnitOfWork unitOf, 
            IMapper mapper)
        {
            this.unitOf = unitOf;
            this.mapper = mapper;
        }

        [Caching(600)]
        [HttpGet]
        public async Task<ActionResult<Pagination>> GetAllProductsAsync([FromQuery]ProductsApiParams productsParams)
        {
            var productSpecification = new ProductSpecification(productsParams);
            var data = mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDTO>>(await unitOf.Repository<Product>().GetAllAsync_withSpecification(productSpecification));

            // If both the properties upon which the filtration works are null, the filtrationExpression will hold null 
            Expression<Func<Product, bool>> filtrationExpression = 
                (productsParams.brandID is not null || productsParams.typeId is not null) ?
                    P => (!productsParams.brandID.HasValue || productsParams.brandID.Value == P.BrandID) && 
                    (!productsParams.typeId.HasValue || productsParams.typeId.Value == P.CategoryID)
                    : null;

            var specificationForCount = new ProductSpecification(filtrationExpression, useIncludes: false);
            int count = await unitOf.Repository<Product>().GetCount(specificationForCount);
            return Ok(new Pagination(productsParams.PageSize, productsParams.PageIndex, count, data));
        }

        [Caching(600)]
        // ProducesResponseType data annotation helps in enhancing the documentation in swagger
        [ProducesResponseType(typeof(ProductDTO), 200)]
        [ProducesResponseType(typeof(ErrorReturn), 404)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductByIDAsync(int id)
        {
            var productSpecification = new ProductSpecification(P => P.ID == id);
            var results = mapper.Map<Product, ProductDTO>(await unitOf.Repository<Product>().GetFirstAsync_withSpecification(productSpecification));
            return Ok(results);
        }

        [Caching(600)]
        //[HttpGet("categories")]
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<Category>>> GetAllCategories()
            => Ok(await unitOf.Repository<Category>().GetAllAsync());

        [Caching(600)]
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<Brand>>> GetAllBrands()
            => Ok(await unitOf.Repository<Brand>().GetAllAsync());



    }
}
