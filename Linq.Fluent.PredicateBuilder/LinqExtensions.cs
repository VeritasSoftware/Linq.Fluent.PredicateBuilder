using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Linq
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> Where<T>(this IEnumerable<T> list, Operation operation, 
                                                params Expression<Func<T, bool>>[] predicates)
        {
            if (predicates == null)
                throw new ArgumentNullException(nameof(predicates));

            var aggregated = predicates.AggregatePredicates<T>(operation);

            return list.Where(aggregated.Compile());
        }

        public static IQueryable<T> Where<T>(this IQueryable<T> list, Operation operation,
                                                params Expression<Func<T, bool>>[] predicates)
        {
            if(predicates == null)
                throw new ArgumentNullException(nameof(predicates));

            var aggregated = predicates.AggregatePredicates<T>(operation);

            return list.Where(aggregated);
        }

        private static Expression<Func<T, bool>> AggregatePredicates<T>(this Expression<Func<T, bool>>[] predicates, 
                                                                            Operation operation)
        {
            return operation switch
            {
                Operation.And => predicates.Aggregate((x, y) => x.And(y)),
                Operation.AndAlso => predicates.Aggregate((x, y) => x.AndAlso(y)),
                Operation.OrElse => predicates.Aggregate((x, y) => x.OrElse(y)),
                _ => predicates.Aggregate((x, y) => x.Or(y))
            };
        }
    }
}
