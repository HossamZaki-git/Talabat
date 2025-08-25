using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Domain_Models;
using Talabat.Core.Domain_Models.Order_Module;
using Talabat.Core.GenericRepository;
using Talabat.Core.Services;
using Product = Talabat.Core.Domain_Models.Product;

namespace Talabat.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration configuration;
        private readonly IBasketsRepository basketsRepository;
        private readonly IUnitOfWork unitOfWork;

        public PaymentService(
            IConfiguration configuration,
            IBasketsRepository basketsRepository,
            IUnitOfWork unitOfWork
            )
        {
            this.configuration = configuration;
            this.basketsRepository = basketsRepository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Basket> Create_UpdatePaymentIntentAsync(string BasketID)
        {
            // Identifies you to the stripe server
            StripeConfiguration.ApiKey = configuration["Stripe:Secretkey"];

            var basket = await basketsRepository.GetBasketAsync( BasketID );
            if (basket is null)
                return basket;

            #region Old Code for checking the correctness of the basket data
            //// Checking the prices of the items in the basket and adjusting them if needed
            //foreach(var item in basket?.Items)
            //{
            //    var product = await unitOfWork.Repository<Product>().GetByIDAsync(item.ID);
            //    if (product is null)
            //        continue;
            //    item.Price = product.Price;
            //}

            //// Setting the delivery method cost if the deliver method id is not null
            //basket.ShippingCost = basket.DeliveryMethodID is not null ?
            //    ( await unitOfWork.Repository<DeliveryMethod>().GetByIDAsync(basket.DeliveryMethodID.Value) ).Cost : 0; 
            #endregion


            var service = new PaymentIntentService();

            if(string.IsNullOrEmpty(basket.paymentIntentId)) // Doesn't have a PaymentIntent object, so we will create one for it
            {
                // carries the PaymentIntentCreation configurations and options
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(basket.Items.Sum(I => I.Price * I.Quantity) + basket.ShippingCost) * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                // Creating the PaymentIntent with those options and configurations
                var paymentIntent = await service.CreateAsync(options);

                // Putting the payment intent object id & the client secret in our basket instance
                basket.paymentIntentId = paymentIntent.Id;
                basket.clientSecret = paymentIntent.ClientSecret;

                // Updating the basket in the in-memory DB with its paymentIntentID & ClientSecret
                await basketsRepository.Create_UpdateAsync(basket);
            }
            else // That basket already has a PaymentIntent and we will just update it
            {
                var options = new PaymentIntentUpdateOptions
                {
                    // Just the amount is what can be changed
                    Amount = (long)(basket.Items.Sum(I => I.Price * I.Quantity) + basket.ShippingCost) * 100
                };
                await service.UpdateAsync(basket.paymentIntentId, options);
            }

            return basket;
        }
    }
}
