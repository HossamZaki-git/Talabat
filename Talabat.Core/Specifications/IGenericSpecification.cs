using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Domain_Models;

namespace Talabat.Core.Specifications
{
    public interface IGenericSpecification<T> where T: BaseModel
    {
        // To carry the lambda expression of the Where operator
        public Expression<Func<T,bool>> Filter { get; set; }
        // To carry the lambda expressions of the Include operators
        public List<Expression<Func<T,object>>> Includes { get; set; }
        // To carry the lambda expression of the OrderBy operator
        public Expression<Func<T, object>> SortingKeyAsc { get; set; }
        // To carry the lambda expression of the OrderByDesc operator
        public Expression<Func<T, object>> SortingKeyDesc { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool AllowsPagination { get; set; }

        public void AddPagination(int pageSize, int pageNumber);
    }
}
