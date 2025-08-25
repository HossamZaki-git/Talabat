using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Domain_Models;
using Talabat.Core.Specifications;

namespace Talabat.Core.GenericRepository
{
    public interface IGenericRepository<T> where T : BaseModel
    {
        public Task<IReadOnlyList<T>> GetAllAsync();
        public Task<T> GetByIDAsync(int ID);
        public Task<IReadOnlyList<T>> GetAllAsync_withSpecification(IGenericSpecification<T> specification);
        public Task<T> GetFirstAsync_withSpecification(IGenericSpecification<T> specification);
        public Task<int> GetCount(IGenericSpecification<T> specification);

        public Task AddAsync(T entity);
        public void Update(T entity);
        public void Delete(T entity);
    }
}
