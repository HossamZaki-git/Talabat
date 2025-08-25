using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Domain_Models;
using Talabat.Core.GenericRepository;
using Talabat.Repository.Data;
using Talabat.Repository.Repositories;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        Dictionary<Type, object> Repositories;
        
        private readonly StoreContext context;

        public UnitOfWork(StoreContext context)
        {
            this.context = context;
            Repositories = new Dictionary<Type, object>();
        }

        public IGenericRepository<T> Repository<T>() where T : BaseModel
        {
            Type type = typeof(T);

            if (Repositories.ContainsKey(type))
                return Repositories[type] as IGenericRepository<T>;

            Repositories.Add(type, new GenericRepository<T>(context));

            return Repositories[type] as IGenericRepository<T>;
        }
        public async Task<int> CompleteAsync()
            => await context.SaveChangesAsync();

        public async ValueTask DisposeAsync()
            => await context.DisposeAsync();
    }
}
