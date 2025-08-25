using System.Text.Json.Serialization;
using Talabat.WebAPI.DTOs;

namespace Talabat.WebAPI.Utilities
{
    public class Pagination
    {
        public int PageSize { get; set; }
        [JsonPropertyName("pageIndex")]
        public int PageNumber { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<ProductDTO> Data { get; set; }
        public Pagination(int PageSize, int PageNumber, int Count, IReadOnlyList<ProductDTO> Data)
        {
            this.PageSize = PageSize;
            this.PageNumber = PageNumber;
            this.Count = Count;
            this.Data = Data;
        }
    }
}
