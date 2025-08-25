using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Domain_Models;
using Talabat.Core.GenericRepository;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    public class GenericSpecification<T> : IGenericSpecification<T> where T : BaseModel
    {
        public Expression<Func<T, bool>> Filter { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> SortingKeyAsc { get; set; }
        public Expression<Func<T, object>> SortingKeyDesc { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool AllowsPagination { get; set; }

        public void AddPagination(int pageSize, int pageNumber)
        {
            Skip = (pageNumber - 1) * pageSize;
            Take = pageSize;
            AllowsPagination = true;
        }

        public GenericSpecification()
        {

        }


        public GenericSpecification(Expression<Func<T,bool>> whereExpression)
        {
            Filter = whereExpression;
        }

    }
}
