using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Domain_Models;
using Talabat.Repository.Data;
using Talabat.WebAPI.Errors;

namespace Talabat.WebAPI.Controllers
{
    public class BuggyController : BaseAPIController
    {
        private readonly StoreContext context;

        public BuggyController(StoreContext context) 
        {
            this.context = context;
        }

        [HttpGet("NotFound")] // Get: baseURL/api/Buggy/NotFound
        public ActionResult NotFoundSample()
            => NotFound(new ErrorReturn(404));

        [HttpGet("BadRequest")] // Get: baseURL/api/Buggy/BadRequest
        public ActionResult BadRequestSample()
            => BadRequest(new ErrorReturn(400));

        [HttpGet("{id}")] // Get: baseURL/api/Buggy/id
        public ActionResult ValidationError(int id)
            => Ok();

        [HttpGet("ServerError")] //  Get: baseURL/api/Buggy/ServerError
        public ActionResult ServerError()
        {
            // No product with such an id
            var product = context.Products.Where(P => P.ID == 100).FirstOrDefault();
            // A null reference exception will be thrown
            return Ok(product.Name);
        }
    }
}
