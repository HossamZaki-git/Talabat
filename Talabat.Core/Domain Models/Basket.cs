using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Domain_Models
{
    public class Basket
    {
        public string ID { get; set; }
        public List<BasketItem> Items { get; set; }
        public int? DeliveryMethodID { get; set; }
        public double ShippingCost { get; set; }
        // A unique identifier for the user to use in the process of buying
        public string? clientSecret { get; set; }
        // An id for the PaymentIntent object (for the payment trial)
        public string? paymentIntentId { get; set; }
    }
}
