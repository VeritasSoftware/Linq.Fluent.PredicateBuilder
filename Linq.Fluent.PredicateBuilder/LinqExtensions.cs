using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Linq
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> Where<T>(this IEnumerable<T> list, Operation operation, 
                                                params Expression<Func<T, bool>>[] predicates)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (predicates == null)
                throw new ArgumentNullException(nameof(predicates));

            var aggregated = predicates.AggregatePredicates<T>(operation);

            return list.Where(aggregated.Compile());
        }

        public static IEnumerable<T> Where<T>(this IEnumerable<T> list, Func<PredicateBuilder<T>, Func<T, bool>> getPredicate)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (getPredicate == null)
                throw new ArgumentNullException(nameof(getPredicate));

            var predicate = getPredicate(new PredicateBuilder<T>());

            if (predicate == null)
                return list;

            return list.Where(predicate);
        }

        public static IQueryable<T> Where<T>(this IQueryable<T> list, Operation operation,
                                                params Expression<Func<T, bool>>[] predicates)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (predicates == null)
                throw new ArgumentNullException(nameof(predicates));

            var aggregated = predicates.AggregatePredicates<T>(operation);

            return list.Where(aggregated);
        }

        public static IQueryable<T> Where<T>(this IQueryable<T> list, Func<PredicateBuilder<T>, Expression<Func<T, bool>>> getPredicate)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if(getPredicate == null)
                throw new ArgumentNullException(nameof(getPredicate));

            var predicate = getPredicate(new PredicateBuilder<T>());

            if (predicate == null)
                return list;

            return list.Where(predicate);
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
