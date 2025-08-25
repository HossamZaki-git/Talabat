using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Domain_Models.Order_Module;

namespace Talabat.Core.Services
{
    public interface IOrderService
    {
        public Task<Order?> CreateOrderAsync(string buyerEmail, string BasketID, Address ShippingAddress, int DeliveryMethodID);
        public Task<IReadOnlyList<Order>> GetUserOrdersAsync(string buyerEmail);
        public Task<Order> GetUserOrderAsync(string buyerEmail, int OrderID);
        public Task<IReadOnlyList<DeliveryMethod>> GetAllDeliveryMethods();
    }
}
