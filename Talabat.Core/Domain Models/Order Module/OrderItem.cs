using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Domain_Models.Order_Module
{
    public class OrderItem : BaseModel
    {
        public OrderItem()
        {
            
        }
        public OrderItem(OrderedProduct orderedProduct, double price, int quantity)
        {
            OrderedProduct = orderedProduct;
            Price = price;
            Quantity = quantity;
        }

        public OrderedProduct OrderedProduct { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }

        public override int GetHashCode()
            => HashCode.Combine(ID);

        public override bool Equals(object? obj)
            => (obj as OrderItem)?.ID == ID;
    }
}
