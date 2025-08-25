using System.Text.Json.Serialization;
using Talabat.Core.Domain_Models;

namespace Talabat.WebAPI.DTOs
{
    public class ProductDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [JsonPropertyName("pictureUrl")]
        public string PictureURL { get; set; }
        public double Price { get; set; }
        [JsonPropertyName("productBrand")]
        public string brand { get; set; }
        public int BrandID { get; set; }
        [JsonPropertyName("productType")]
        public string category { get; set; }
        public int CategoryID { get; set; }
    }
}
