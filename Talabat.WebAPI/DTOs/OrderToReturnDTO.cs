using System.Text.Json.Serialization;
using Talabat.Core.Domain_Models.Order_Module;

namespace Talabat.WebAPI.DTOs
{
    public class OrderToReturnDTO
    {
        public int ID { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public string Status { get; set; }
        [JsonPropertyName("shipToAddress")]
        public Address ShippingAddress { get; set; }
        [JsonPropertyName("deliveryMethod")]
        public string DeliveryMethod_name { get; set; }
        [JsonPropertyName("deliveryCost")]
        public double DeliveryMethod_cost { get; set; }
        public ICollection<OrderItemDTO> Items { get; set; }
        public double SubTotal { get; set; }
        public double Total { get; set; }
        public string PaymentIntentID { get; set; }
    }
}
