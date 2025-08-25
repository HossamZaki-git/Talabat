using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Domain_Models.Order_Module
{
    public enum OrderStatus
    {
        // This [EnumMember(Value = "...")] => Makes this value the one that will be used when serializing the enum member
        //                                     instead of the default numeric values (0,1,2,3,...)
        [EnumMember(Value = "Pending")]
        Pending,
        [EnumMember(Value = "Payment Succeeded")]
        PaymentSucceeded,
        [EnumMember(Value = "Payment Failed")]
        PaymentFailed
    }
}
