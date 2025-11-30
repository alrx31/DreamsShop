using System.Linq.Expressions;

namespace Domain.Specifications;

public class NotSpetification<T>(Specification<T> spetification) : Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        Expression<Func<T, bool>> expression = spetification.ToExpression();

        ParameterExpression param = expression.Parameters.FirstOrDefault()
            ?? throw new Exception("Parameter is null!");

        return Expression.Lambda<Func<T, bool>>(
            Expression.Not(expression.Body), param);
    }
}