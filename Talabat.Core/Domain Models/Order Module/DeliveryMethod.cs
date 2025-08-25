using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Domain_Models.Order_Module
{
    public class DeliveryMethod : BaseModel
    {
        public DeliveryMethod()
        {
            
        }
        public DeliveryMethod(string shortName, string description, double cost, string deliveryTime)
        {
            ShortName = shortName;
            Description = description;
            Cost = cost;
            DeliveryTime = deliveryTime;
        }

        public string ShortName { get; set; }
        public string Description { get; set; }
        public double Cost { get; set; }
        public string DeliveryTime { get; set; }
    }
}
