using System.Linq.Expressions;
using System.Reflection;

namespace Domain.Specifications;

public class ValueSpecification<T,K>(Expression<Func<T, K>> keySelector, K[] values) : Specification<T>
{
    private static MethodInfo ContainsMethodInfo => typeof(List<K>).GetMethod(nameof(List<K>.Contains))!;

    public override Expression<Func<T, bool>> ToExpression()
    {
        var call = Expression.Call(
                    Expression.Constant(values),
                    ContainsMethodInfo,
                    (MemberExpression)keySelector.Body);

        return Expression.Lambda<Func<T, bool>>(call, keySelector.Parameters);
    }
}
