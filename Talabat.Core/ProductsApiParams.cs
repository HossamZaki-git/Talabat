using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Talabat.Core
{
    public class ProductsApiParams
    {
        [JsonPropertyName("sort")]
        public string? sort { get; set; }
        public int? brandID { get; set; }

        // was categoryID
        public int? typeId { get; set; }

        private int pageIndex = 1;

        // was PageNumber
        public int PageIndex
        {
            get { return pageIndex; }
            set { pageIndex = value < 1 ? 1 : value; }
        }

        private int pageSize = 5;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value < 1 || value > 100) ? 5 : value; }
        }
        
        private string? search;

        [JsonPropertyName("search")]
        // was NameSubString
        public string? Search
        {
            get { return search; }
            set { search = value?.ToLower(); }
        }



    }
}
