using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Talabat.WebAPI.DTOs
{
    public class BasketItemDTO
    {
        [Required]
        public int ID { get; set; }
        [Required]
        [JsonPropertyName("productName")]
        public string Name { get; set; }
        [Required]
        [Range(0.25, double.MaxValue, ErrorMessage = "The minimum allowed price is 0.25 EGP")]
        public double Price { get; set; }
        [Required]
        public string pictureUrl { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        [JsonPropertyName("Type")]
        public string Category { get; set; }
    }
}
