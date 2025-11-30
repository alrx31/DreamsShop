using System;
using System.Linq.Expressions;

namespace Domain.Specifications;

public class OrSpetification<T>(Spetification<T> left, Spetification<T> right) : Spetification<T>
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        Expression<Func<T, bool>> leftExpression = left.ToExpression();
        Expression<Func<T, bool>> rightExpression = right.ToExpression();

        ParameterExpression leftParam = leftExpression.Parameters.FirstOrDefault()
            ?? throw new Exception("Left parameter is null!");

        if (rightExpression.Parameters.FirstOrDefault() == null)
            throw new Exception("Right parameter is null!");

        if (ReferenceEquals(leftParam, rightExpression.Parameters.FirstOrDefault()))
        {
            return Expression.Lambda<Func<T, bool>>(
                Expression.OrElse(leftExpression.Body, rightExpression.Body), leftParam);
        }

        return Expression.Lambda<Func<T, bool>>(
            Expression.OrElse(
                leftExpression.Body,
                Expression.Invoke(rightExpression, leftParam)), leftParam);
    }
}
