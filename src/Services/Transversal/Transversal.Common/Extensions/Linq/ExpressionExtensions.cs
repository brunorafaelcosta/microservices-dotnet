using System;
using System.Linq.Expressions;

namespace Transversal.Common.Extensions.Linq
{
    /// <summary>
    /// Provides additional methods to <see cref="Expression{TDelegate}"/>.
    /// </summary>
    public static class ExpressionExtensions
    {
        #region Combine expressions
        public static Expression<Func<T, bool>> CombineWithExpression<T>(this Expression<Func<T, bool>> baseExpression, Expression<Func<T, bool>> otherExpression)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(baseExpression.Parameters[0], parameter);
            var left = leftVisitor.Visit(baseExpression.Body);

            var rightVisitor = new ReplaceExpressionVisitor(otherExpression.Parameters[0], parameter);
            var right = rightVisitor.Visit(otherExpression.Body);

            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left, right), parameter);
        }
        class ReplaceExpressionVisitor : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression Visit(Expression node)
            {
                if (node == _oldValue)
                {
                    return _newValue;
                }

                return base.Visit(node);
            }
        }
        #endregion
    }
}
