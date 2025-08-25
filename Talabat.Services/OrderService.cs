using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Domain_Models;
using Talabat.Core.Domain_Models.Order_Module;
using Talabat.Core.GenericRepository;
using Talabat.Core.Services;
using Talabat.Repository;

namespace Talabat.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IBasketsRepository basketsRepository;

        public OrderService(IUnitOfWork unitOfWork, IBasketsRepository basketsRepository)
        {
            this.unitOfWork = unitOfWork;
            this.basketsRepository = basketsRepository;
        }

        /*
            - Creating an Order and adding it to the DB
            - The order items are fetched from the basket from the in-memory redis DB
            - The values of the items are checked using the products data in the DB
         */
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string BasketID, Address ShippingAddress, int DeliveryMethodID)
        {
            var basket = await basketsRepository.GetBasketAsync(BasketID);
            var deliveryMethod = await unitOfWork.Repository<DeliveryMethod>().GetByIDAsync(DeliveryMethodID);

            if (basket is null || string.IsNullOrEmpty(basket.paymentIntentId))
                return null;

            var orderWithSame_PaymentIntentID = await unitOfWork.Repository<Order>().GetFirstAsync_withSpecification(new OrdersSpecifications(O => O.PaymentIntentID == basket.paymentIntentId));

            // If there is an already existing order with the same paymentIntentID for the same user, delete it to add the new order for that same user.
            if (orderWithSame_PaymentIntentID?.BuyerEmail == buyerEmail)
                unitOfWork.Repository<Order>().Delete(orderWithSame_PaymentIntentID);
            else if (orderWithSame_PaymentIntentID is not null) // If that user is trying to add his order with a PaymentIntentID of another user, don't add it and return null
                return null;

            List<OrderItem> orderItems = new List<OrderItem>();

            // To carry the product details (coming from the DB)
            Product product;
            OrderedProduct orderedProduct;

            // setting the ordered items with the data from the DB
            foreach(var item in basket.Items)
            {
                product = await unitOfWork.Repository<Product>().GetByIDAsync(item.ID);
                if (product is null)
                    continue;

                orderedProduct = new OrderedProduct(item.ID, product.Name, product.PictureURL);
                orderItems.Add(new OrderItem(orderedProduct, product.Price, item.Quantity));
            }

            if (orderItems.Count == 0)
                return null;

            var order = new Order(buyerEmail, ShippingAddress, deliveryMethod, orderItems, orderItems.Sum(OI => OI.Price * OI.Quantity), basket.paymentIntentId);

            await unitOfWork.Repository<Order>().AddAsync(order);

            return await unitOfWork.CompleteAsync() != 0 ? order : null;

        }
        public async Task<IReadOnlyList<Order>> GetUserOrdersAsync(string buyerEmail)
        {
            var Specifications = new OrdersSpecifications(
                FilteringExpression: O => O.BuyerEmail == buyerEmail,
                IncludeDeliveryMethod: true,
                IncludeItems: true,
                OrderingKey: O => O.OrderDate
               );

            return await unitOfWork.Repository<Order>().GetAllAsync_withSpecification( Specifications );
        }
        public async Task<Order> GetUserOrderAsync(string buyerEmail, int OrderID)
        {
            var Specifications = new OrdersSpecifications(
                FilteringExpression: O => O.BuyerEmail == buyerEmail && O.ID == OrderID,
                IncludeDeliveryMethod: true,
                IncludeItems: true
                );

            return await unitOfWork.Repository<Order>().GetFirstAsync_withSpecification(Specifications);
        }
        public async Task<IReadOnlyList<DeliveryMethod>> GetAllDeliveryMethods()
            => await unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
    }
}
