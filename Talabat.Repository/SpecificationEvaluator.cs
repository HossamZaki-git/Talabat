using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Domain_Models;
using Talabat.Core.GenericRepository;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    public static class SpecificationEvaluator<T> where T : BaseModel
    {
        public static IQueryable<T> Evaluate(IQueryable<T> data, IGenericSpecification<T> specification)
        {
            if (specification.Filter is not null)
                data = data.Where(specification.Filter);

            if (specification.SortingKeyAsc is not null)
                data = data.OrderBy(specification.SortingKeyAsc);

            if (specification.SortingKeyDesc is not null)
                data = data.OrderByDescending(specification.SortingKeyDesc);

            if (specification.AllowsPagination)
                data = data.Skip(specification.Skip).Take(specification.Take);

            data = specification.Includes.Aggregate(data, (accumulated, includeExpression) => accumulated.Include(includeExpression));
            return data;
        }
    }
}
