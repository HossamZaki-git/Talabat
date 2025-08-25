using System.Text.Json.Serialization;
using Talabat.Core.Domain_Models.Order_Module;

namespace Talabat.WebAPI.DTOs
{
    public class OrderItemDTO
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        [JsonPropertyName("pictureUrl")]
        public string ImageURL { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}