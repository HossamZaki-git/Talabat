using AutoMapper;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.Core;
using Talabat.Core.Domain_Models;
using Talabat.Core.Domain_Models.Order_Module;
using Talabat.Core.Services;
using Talabat.Repository;
using Talabat.WebAPI.DTOs;
using Talabat.WebAPI.Errors;

namespace Talabat.WebAPI.Controllers
{
    [Authorize]
    public class PaymentsController : BaseAPIController
    {
        private readonly IPaymentService paymentService;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        string webHookSecret = "whsec_bbb4442c141a7ae889f1b9c1080a0f63d973b04f0c46fe44272866de173f65a9";

        public PaymentsController(IPaymentService paymentService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.paymentService = paymentService;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        [ProducesResponseType(typeof(Basket), 200)]
        [ProducesResponseType(typeof(ErrorReturn), 400)]
        [HttpPost("{basketID}")]
        public async Task<ActionResult<Basket>> Create_UpdatePaymentIntent(string basketID)
        {
            var basket = await paymentService.Create_UpdatePaymentIntentAsync(basketID);
            return basket is not null ? Ok(basket) : BadRequest(new ErrorReturn(400, "There is a problem with the basket you are referring to"));
        }

        [AllowAnonymous]
        [HttpPost("WebHook")] // stripe listen -f https://localhost:7092/api/Payments/WebHook -e payment_intent.succeeded,payment_intent.payment_failed

        public async Task<ActionResult> StripeWebHook()
        {
            Event stripeEvent;
            // The body of the request carries the stripe Event object as JSON
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                // The Stripe-Signature header shall contains the signature of the stripe server
                // The webHookSecret is a secret key valid for 90 days that identifies a valid connection between stripe's server and this endpoint
                stripeEvent = EventUtility.ConstructEvent(json, HttpContext.Request.Headers["Stripe-Signature"], webHookSecret);
            }
            catch(Exception e)
            {
                // If an exception was thrown from the above line, so this request isn't from stripe or the webHookSecret was expired
                return BadRequest();
            }
            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
            var order = await unitOfWork.Repository<Order>().GetFirstAsync_withSpecification(new OrdersSpecifications(O => O.PaymentIntentID == paymentIntent.Id));

            if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
                order.Status = OrderStatus.PaymentSucceeded;

            else if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
                order.Status = OrderStatus.PaymentFailed;

            unitOfWork.Repository<Order>().Update(order);
            await unitOfWork.CompleteAsync();

            return Ok();
        }
    }
}
