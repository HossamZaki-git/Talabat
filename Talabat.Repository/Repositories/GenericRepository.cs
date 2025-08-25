using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Domain_Models;
using Talabat.Core.GenericRepository;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseModel
    {
        private readonly StoreContext context;

        public GenericRepository(StoreContext context)
        {
            this.context = context;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return typeof(T) == typeof(Product) ? 
                (IReadOnlyList<T>)(await context.Products.Include(P => P.brand).Include(P => P.category).ToListAsync()) 
                : await context.Set<T>().ToListAsync();
        }


        public async Task<T> GetByIDAsync(int ID)
            => await context.Set<T>().FindAsync(ID);

        public async Task<IReadOnlyList<T>> GetAllAsync_withSpecification(IGenericSpecification<T> specification)
            => await SpecificationEvaluator<T>.Evaluate(context.Set<T>(), specification).ToListAsync();

        public async Task<T> GetFirstAsync_withSpecification(/*int ID, */IGenericSpecification<T> specification)
            => await SpecificationEvaluator<T>.Evaluate(context.Set<T>(), specification).FirstOrDefaultAsync();

        public async Task<int> GetCount(IGenericSpecification<T> specification)
            => await SpecificationEvaluator<T>.Evaluate(context.Set<T>(), specification).CountAsync();

        public async Task AddAsync(T entity)
            => await context.Set<T>().AddAsync(entity);

        public void Update(T entity)
            => context.Set<T>().Update(entity);

        public void Delete(T entity)
            => context.Set<T>().Remove(entity);
    }
}
