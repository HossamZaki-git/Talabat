using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Talabat.Core.Domain_Models;

namespace Talabat.WebAPI.DTOs
{
    public class BasketDTO
    {
        [Required]
        public string ID { get; set; }
        [Required]
        public List<BasketItemDTO> Items { get; set; }
        public int? DeliveryMethodID { get; set; }
        [JsonPropertyName("shippingPrice")]
        public double ShippingCost { get; set; }
        // A unique identifier for the user to use in the process of buying
        public string? clientSecret { get; set; }
        // An id for the PaymentIntent object (for the payment trial)
        public string? paymentIntentId { get; set; }
    }
}
