using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.WebAPI.Errors;

namespace Talabat.WebAPI.Controllers
{
    // statusCode is just a placeholder for a value
    [Route("NonExisting/{statusCode}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)] 
    // ApiExplorerSettings controls the visibility of the controller
    // we set IgnoreApi = true to make the swagger work, bec. it won't work without specifying the http verb of the endpoint
    public class NonExistingEndpointsController : ControllerBase
    {
        // statuseCode will take its value from the parameter
        public ActionResult NonExisting([FromRoute]int statusCode)
            => NotFound(new ErrorReturn(statusCode));
    }
}
