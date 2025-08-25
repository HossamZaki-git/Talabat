using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Domain_Models;

namespace Talabat.Core.GenericRepository
{
    public interface IBasketsRepository
    {
        public Task<Basket> GetBasketAsync(string ID);
        public Task<bool> DeleteBasketAsync(string ID);
        public Task<Basket> Create_UpdateAsync(Basket basket);
    }
}
