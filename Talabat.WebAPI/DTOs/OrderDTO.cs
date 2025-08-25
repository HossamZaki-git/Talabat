using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Talabat.WebAPI.DTOs
{
    public class OrderDTO
    {
        [Required]
        public string BasketID { get; set; }
        public int DeliveryMethodID { get; set; }
        [JsonPropertyName("shipToAddress")]
        public AddressDTO ShippingAddress { get; set; }
    }
}
