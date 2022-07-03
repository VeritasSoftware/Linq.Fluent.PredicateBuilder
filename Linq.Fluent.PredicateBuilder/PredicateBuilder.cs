using System.Linq.Expressions;

namespace System.Linq
{
    public class PredicateBuilder<T> : IPredicateBuilderOperations<T>
    {
        Expression<Func<T, bool>> _predicate = null;

        public IPredicateBuilderOperations<T> Initial(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            _predicate = predicate;

            return this;
        }

        public IPredicateBuilderOperations<T> And(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            _predicate = _predicate.And(predicate);

            return this;
        }

        public IPredicateBuilderOperations<T> AndAlso(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            _predicate = _predicate.AndAlso(predicate);

            return this;
        }

        public IPredicateBuilderOperations<T> Or(Expression<Func<T, bool>> predicate)
        {
            if(predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            _predicate = _predicate.Or(predicate);

            return this;
        }

        public IPredicateBuilderOperations<T> OrElse(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            _predicate = _predicate.OrElse(predicate);

            return this;
        }

        public Func<T, bool> ToPredicate()
        {
            return _predicate?.Compile();
        }

        public Expression<Func<T, bool>> ToExpressionPredicate()
        {
            return _predicate;
        }
    }
}
