using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Domain_Models.Order_Module
{
    public class OrderedProduct
    {
        public OrderedProduct()
        {
            
        }
        public OrderedProduct(int iD, string name, string imageURL)
        {
            ID = iD;
            Name = name;
            ImageURL = imageURL;
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
    }
}
