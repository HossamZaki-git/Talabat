using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Domain_Models
{
    public class BasketItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string PictureURL { get; set; }
        public int Quantity { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
    }
}
