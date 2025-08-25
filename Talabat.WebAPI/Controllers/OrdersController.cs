using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using StackExchange.Redis;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using Talabat.Core;
using Talabat.Core.Domain_Models.Order_Module;
using Talabat.Core.GenericRepository;
using Talabat.Core.Services;
using Talabat.WebAPI.DTOs;
using Talabat.WebAPI.Errors;

namespace Talabat.WebAPI.Controllers
{
    [Authorize]
    public class OrdersController : BaseAPIController
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            this.orderService = orderService;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<OrderToReturnDTO>> CreateOrder(OrderDTO orderDTO)
        {

            var ShippingAddress = mapper.Map<AddressDTO, Address>(orderDTO.ShippingAddress);

            var order = await orderService.CreateOrderAsync(User.FindFirstValue(ClaimTypes.Email), orderDTO.BasketID, ShippingAddress, orderDTO.DeliveryMethodID);

            return order is null ? BadRequest(new ErrorReturn(400)) : mapper.Map<Core.Domain_Models.Order_Module.Order, OrderToReturnDTO>(order);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDTO>>> GetUserOrders()
        {
            var orders = await orderService.GetUserOrdersAsync(User.FindFirstValue(ClaimTypes.Email));
            return Ok(mapper.Map<IReadOnlyList<Core.Domain_Models.Order_Module.Order>, IReadOnlyList<OrderToReturnDTO>>(orders));
        }

        [ProducesResponseType(typeof(OrderToReturnDTO), 200)]
        [ProducesResponseType(typeof(ErrorReturn), 400)]
        [HttpGet("{ID}")]
        public async Task<ActionResult<OrderToReturnDTO>> GetUserOrderByID(int ID)
        {
            var order = await orderService.GetUserOrderAsync(User.FindFirstValue(ClaimTypes.Email), ID);
            return order is null ? NotFound(new ErrorReturn(400, "That product isn't found")) : 
                Ok(mapper.Map< Core.Domain_Models.Order_Module.Order , OrderToReturnDTO> (order));
        }

        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetAllDeliveryMethods()
            => Ok(await orderService.GetAllDeliveryMethods());
    }
}
