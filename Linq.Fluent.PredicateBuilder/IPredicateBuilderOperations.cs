using System;
using System.Linq.Expressions;

namespace System.Linq
{
    public interface IPredicateBuilderOperations<T>
    {
        IPredicateBuilderOperations<T> And(Expression<Func<T, bool>> predicate);
        IPredicateBuilderOperations<T> And(bool condition, Expression<Func<T, bool>> predicate);
        IPredicateBuilderOperations<T> AndAlso(Expression<Func<T, bool>> predicate);
        IPredicateBuilderOperations<T> AndAlso(bool condition, Expression<Func<T, bool>> predicate);
        IPredicateBuilderOperations<T> Or(Expression<Func<T, bool>> predicate);
        IPredicateBuilderOperations<T> Or(bool condition, Expression<Func<T, bool>> predicate);
        IPredicateBuilderOperations<T> OrElse(Expression<Func<T, bool>> predicate);
        IPredicateBuilderOperations<T> OrElse(bool condition, Expression<Func<T, bool>> predicate);
        Func<T, bool> ToPredicate();
        Expression<Func<T, bool>> ToExpressionPredicate();
    }
}
