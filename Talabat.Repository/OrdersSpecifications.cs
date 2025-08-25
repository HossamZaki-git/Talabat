using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Domain_Models.Order_Module;

namespace Talabat.Repository
{
    public class OrdersSpecifications : GenericSpecification<Core.Domain_Models.Order_Module.Order>
    {
        public OrdersSpecifications(
            Expression<Func<Core.Domain_Models.Order_Module.Order, bool>> FilteringExpression = null,
            bool IncludeItems = false,
            bool IncludeDeliveryMethod = false,
            Expression<Func<Core.Domain_Models.Order_Module.Order, object>> OrderingKey = null,
            bool OrderAsc = false
            )
        {
            Filter = FilteringExpression;
            if(IncludeItems)
                Includes.Add(O => O.Items);
            if(IncludeDeliveryMethod)
                Includes.Add(O => O.DeliveryMethod);
            if(OrderingKey is not null)
            {
                if (OrderAsc)
                    SortingKeyAsc = OrderingKey;
                else
                    SortingKeyDesc = OrderingKey;
            }
        }
    }
}
