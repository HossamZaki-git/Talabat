using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Domain_Models;

namespace Talabat.Core.Services
{
    public interface IPaymentService
    {
        public Task<Basket> Create_UpdatePaymentIntentAsync(string BasketID);
    }
}
